using AppyNox.Services.Base.API.Authentication;
using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Extensions;
using AppyNox.Services.Base.API.Filters;
using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.API.Middleware.Options;
using AppyNox.Services.Base.API.Permissions;
using AppyNox.Services.Base.Application.Interfaces.Authentication;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Coupon.Application;
using AppyNox.Services.Coupon.Domain;
using AppyNox.Services.Coupon.Infrastructure;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.WebAPI.Permission;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

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

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddMvc(options =>
{
    options.Conventions.Add(new VersionByNamespaceConvention());
});

builder.ConfigureRedis(configuration);

#endregion

#region [ Dependency Injection For Layers ]

noxLogger.LogInformation("Registering DI's for layers.");
builder.Services.AddCouponInfrastructure(configuration, noxLogger);
builder.Services.AddCouponApplication();
noxLogger.LogInformation("Registering DI's for layers completed.");

#endregion

#region [ JWT Configuration ]

noxLogger.LogInformation("Registering JWT Configuration.");
JwtConfiguration jwtConfiguration = new();
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

if (builder.Environment.IsDevelopment())
{
    noxLogger.LogInformation("Adjusting swagger endpoints.");
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.OperationFilter<DynamicRequestBodyOperationFilter>();
        opt.SwaggerDoc($"v{NoxVersions.v1_0}", new OpenApiInfo { Title = "Coupon Service", Version = NoxVersions.v1_0 });
        opt.SwaggerDoc($"v{NoxVersions.v1_1}", new OpenApiInfo { Title = "Coupon Service", Version = NoxVersions.v1_1 });
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

        opt.DocInclusionPredicate((version, apiDesc) =>
        {
            if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

            var versions = methodInfo.DeclaringType
                .GetCustomAttributes(true)
                .OfType<ApiVersionAttribute>()
                .SelectMany(attr => attr.Versions);

            return versions.Any(v => $"v{v}" == version);
        });
    });
    noxLogger.LogInformation("Adjusting swagger endpoints completed.");
}

// Add Policy-based Authorization
builder.Services.AddAuthorization(options =>
{
    List<string> _claims = [.. Permissions.Coupons.Metrics];
    List<string> _adminclaims = [.. Permissions.CouponsAdmin.Metrics];

    foreach (var item in _claims)
    {
        options.AddPolicy(item, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(item, "API.Permission"));
        });
    }

    // Admin only policies
    foreach (var item in _adminclaims)
    {
        options.AddPolicy($"{item}.Admin", builder =>
        {
            builder.AddRequirements(new PermissionRequirement(item, "API.Permission"))
            .AddRequirements(new PermissionRequirement("admin", "role"));
        });
    }

});

builder.Services.AddScoped<IAuthorizationHandler, NoxJwtAuthorizationHandler>();
builder.Services.AddScoped<INoxTokenManager, NoxTokenManager>();
noxLogger.LogInformation("Registering JWT Configuration completed.");

#endregion

#region [ Localization Configuration ]

builder.ConfigureLocalization();

#endregion

var app = builder.Build();

#region [ Localization Services ]

IStringLocalizerFactory localizerFactory = app.Services.GetRequiredService<IStringLocalizerFactory>();
localizerFactory.AddNoxLocalizationServices();

localizerFactory.AddCouponDomainLocalizationService();

#endregion

#region [ Pipeline ]

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Coupon Service v1.0");
        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Coupon Service v1.1");
    });
}

app.UseRequestLocalization();

app.UseNoxContext();

app.UseNoxResponseWrapper(new NoxResponseWrapperOptions
{
    ApiVersion = NoxVersions.v1_0,
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

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