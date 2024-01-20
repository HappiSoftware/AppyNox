using AppyNox.Services.Base.API.Extensions;
using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Base.API.Permissions;
using AppyNox.Services.Coupon.Application;
using AppyNox.Services.Coupon.Infrastructure;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AutoWrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using AppyNox.Services.Coupon.WebAPI.Permission;
using AppyNox.Services.Base.Application.Interfaces.Authentication;
using AppyNox.Services.Base.API.Authentication;
using AppyNox.Services.Authentication.WebAPI.Permission;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

#region [ Configuration Service ]

await builder.AddConsulConfiguration("CouponService");
var configuration = builder.Configuration;

#endregion

#region [ Logger Setup ]

builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
);

builder.Services.AddSingleton<INoxApiLogger, NoxApiLogger>();

#region [ Logger for Before DI Initialization ]

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
var logger = loggerFactory.CreateLogger<INoxLogger>();
NoxLogger noxLogger = new(logger, "CouponHost");

#endregion

#endregion

#region [ Configure Services ]

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

builder.Services.AddHttpContextAccessor();

#endregion

#region [ Dependency Injection For Layers ]

noxLogger.LogInformation("Registering DI's for layers.");
builder.Services.AddCouponInfrastructure(builder, builder.Environment.GetEnvironment(), noxLogger);
builder.Services.AddCouponApplication();
noxLogger.LogInformation("Registering DI's for layers completed.");

#endregion

#region [ JWT Configuration ]

noxLogger.LogInformation("Registering JWT Configuration.");
var jwtConfiguration = new JwtConfiguration();
configuration.GetSection("JwtSettings").Bind(jwtConfiguration);
builder.Services.AddSingleton(jwtConfiguration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "NoxJwtScheme";
    options.DefaultChallengeScheme = "NoxJwtScheme";
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfiguration.Issuer,
        ValidAudience = jwtConfiguration.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.GetSecretKeyBytes())
    };
})
.AddScheme<AuthenticationSchemeOptions, NoxJwtAuthenticationHandler>("NoxJwtScheme", options =>
{
});

if (!builder.Environment.IsDevelopment())
{
    noxLogger.LogInformation("Adjusting swagger endpoints.");
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Coupons Service", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    });
    noxLogger.LogInformation("Adjusting swagger endpoints completed.");
}

// Add Policy-based Authorization
builder.Services.AddAuthorization(options =>
{
    List<string> _claims = [.. Permissions.Coupons.Metrics];

    foreach (var item in _claims)
    {
        options.AddPolicy(item, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(item, "API.Permission"));
        });
    }
});

builder.Services.AddScoped<IAuthorizationHandler, NoxJwtAuthorizationHandler>();
builder.Services.AddScoped<INoxTokenManager, NoxTokenManager>();
noxLogger.LogInformation("Registering JWT Configuration completed.");

#endregion

var app = builder.Build();

#region [ Pipeline ]

if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = true, ShowApiVersion = true, ApiVersion = "1.0" });
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<UserIdMiddleware>();

app.MapControllers();

app.UseMiddleware<QueryParameterValidateMiddleware>();

app.UseHealthChecks("/api/health");

#endregion

#region [ Hosted Services ]

var consulHostedService = app.Services.GetServices<IHostedService>()
    .OfType<ConsulHostedService>()
    .First();

consulHostedService.OnConsulConnectionFailed += (Exception ex) =>
{
    noxLogger.LogError(ex, "Consul connection failed. Stopping application.");
    var lifeTime = app.Services.GetService<IHostApplicationLifetime>();
    lifeTime?.StopApplication();
    return Task.CompletedTask;
};

#endregion

app.Services.ApplyMigrations<CouponDbContext>();

app.Run();