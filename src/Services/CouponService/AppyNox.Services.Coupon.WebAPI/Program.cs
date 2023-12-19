using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Coupon.WebAPI.Helpers;
using AppyNox.Services.Coupon.WebAPI.Helpers.Permissions;
using AutoWrapper;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region [ SSL Configuration ]

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    string fileName = string.Empty;

    if (builder.Environment.IsDevelopment())
    {
        fileName = Directory.GetCurrentDirectory() + "/ssl/coupon-service.pfx";
    }
    else if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
    {
        fileName = "/https/coupon-service.pfx";
    }

    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps(fileName ?? throw new InvalidOperationException("SSL certificate file path could not be determined."), "happi2023");
    });
});

#endregion

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region [ Logger Setup ]

if (!builder.Environment.IsDevelopment())
{
    NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config");
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
}

#endregion

#region [ Consul Discovery Service ]

builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    var address = configuration["ConsulConfig:Address"] ?? "http://localhost:8500";
    consulConfig.Address = new Uri(address);
}));
builder.Services.AddSingleton<IHostedService, ConsulHostedService>();
builder.Services.Configure<ConsulConfig>(configuration.GetSection("consul"));

#endregion

builder.Services.AddHealthChecks();

AppyNox.Services.Coupon.Infrastructure.DependencyInjection.ConfigureServices(builder.Services, configuration, builder.Environment.GetEnvironment());
AppyNox.Services.Coupon.Application.DependencyInjection.ConfigureServices(builder.Services, configuration);

#region [ JWT Configuration ]

var jwtConfiguration = new JwtConfiguration();
configuration.GetSection("JwtSettings").Bind(jwtConfiguration);
builder.Services.AddSingleton(jwtConfiguration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
});

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

builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = true, ShowApiVersion = true, ApiVersion = "1.0" });
app.UseMiddleware<QueryParameterValidateMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
AppyNox.Services.Coupon.Infrastructure.DependencyInjection.ApplyMigrations(app.Services);

app.UseHealthChecks("/health-check");

app.Run();