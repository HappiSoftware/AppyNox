using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure;
using AppyNox.Services.Base.Infrastructure.Helpers;
using AppyNox.Services.Coupon.Infrastructure;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.WebAPI.Helpers;
using AppyNox.Services.Coupon.WebAPI.Helpers.Permissions;
using AutoWrapper;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Create a temporary LoggerFactory for early-stage logging
var tempLoggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog("nlog.config");
    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information); // Adjust the log level as needed
});
var tempLogger = tempLoggerFactory.CreateLogger<Program>();

try
{
    // Add services to the container.
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    #region [ Logger Setup ]

    NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config");
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    #endregion

    #region [ Consul Discovery Service ]

    //logger.Log(NLog.LogLevel.Info, $"Configuring Consul Discovery Service {configuration["ConsulConfig:Address"]}");
    builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
    {
        var address = configuration["ConsulConfig:Address"] ?? "http://localhost:8500";
        consulConfig.Address = new Uri(address);
    }));
    builder.Services.AddSingleton<IHostedService, ConsulHostedService>();
    builder.Services.Configure<ConsulConfig>(configuration.GetSection("consul"));

    #endregion

    builder.Services.AddHealthChecks();

    builder.Services.AddCouponInfrastructure(configuration, builder.Environment.GetEnvironment(), tempLogger);
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

    app.UseHealthChecks("/api/health");

    var databaseStartupService = app.Services.GetServices<IHostedService>()
        .OfType<DatabaseStartupHostedService<CouponDbContext>>()
        .FirstOrDefault()!;

    databaseStartupService.OnDatabaseConnected += () =>
    {
        app.Services.ApplyMigrations<CouponDbContext>();
        return Task.CompletedTask;
    };

    databaseStartupService.OnDatabaseConnectionFailed += () =>
    {
        var lifeTime = app.Services.GetService<IHostApplicationLifetime>();
        lifeTime?.StopApplication();
        return Task.CompletedTask;
    };

    app.Run();
}
catch (Exception ex)
{
    tempLogger.LogError(ex, "An error occurred during application startup");
    throw;
}
finally
{
    tempLoggerFactory.Dispose();
}