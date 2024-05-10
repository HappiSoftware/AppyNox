using AppyNox.Services.Base.Application.Interfaces.Authentication;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Authentication;
using AppyNox.Services.Base.Infrastructure.BackgroundJobs;
using AppyNox.Services.Base.Infrastructure.Configuration;
using AppyNox.Services.Base.Infrastructure.Data;
using AppyNox.Services.Base.Infrastructure.Data.Interceptors;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services;
using AppyNox.Services.Base.Infrastructure.Services.CacheServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Consul;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using StackExchange.Redis;

namespace AppyNox.Services.Base.Infrastructure;

public class InfrastructureSetupOptions
{
#nullable disable // Be cautious while adding new parameters with required. Need to check these properties in AddInfrastructureServices
    public string DbContextAssemblyName { get; set; }
    public bool UseOutBoxMessageMechanism { get; set; } = false;
    public int OutBoxMessageJobIntervalSeconds { get; set; } = 30;
    public bool UseEncryption {  get; set; } = false;
    public bool UseConsul { get; set; } = false;
    public bool UseRedis { get; set; } = false;
    public bool UseJwtAuthentication { get; set; } = false;
    /// <summary>
    /// If UseJwtAuthentication is true and RegisterIAuthorizationHandler is false, DO NOT forget to register IAuthorizationHandler to D.I. as 'scoped'
    /// </summary>
    public bool RegisterIAuthorizationHandler { get; set; } = false;
    public List<string> Claims { get; set; } = [];
    public List<string> AdminClaims { get; set; } = [];
    public IConfiguration Configuration { get; set; }
#nullable enable
}


public static class InfrastructureServiceBuilder
{
    public static IServiceCollection AddInfrastructureServices<TContext>(
        this IServiceCollection services,
        INoxLogger logger,
        Action<InfrastructureSetupOptions> configureOptions) where TContext : NoxDatabaseContext
    {
        var options = new InfrastructureSetupOptions();
        configureOptions(options);

        if (string.IsNullOrEmpty(options.DbContextAssemblyName))
            throw new InvalidOperationException("DbContextAssemblyName must be provided.");
        if (options.Configuration == null)
            throw new InvalidOperationException("Configuration must be provided.");

        services.AddSingleton<INoxInfrastructureLogger, NoxInfrastructureLogger>();
        services.AddSingleton<INoxApplicationLogger, NoxApplicationLogger>();
        services.AddSingleton<INoxApiLogger, NoxApiLogger>();
        logger.LogInformation("Loggers enabled...");

        if (options.UseConsul)
        {
            services.ConfigureConsulServices(options);
            logger.LogInformation("Consul enabled...");
        }

        services.ConfigureDatabase<TContext>(options);
        logger.LogInformation("Database connection enabled...");

        if(options.UseEncryption)
        {
            services.AddSingleton<IEncryptionService, EncryptionService>();
            logger.LogInformation("Encryption enabled...");
        }

        if (options.UseOutBoxMessageMechanism)
        {
            services.ConfigureOutBoxMessageJob<TContext>(options);
            logger.LogInformation("OutBoxMessageJob enabled...");
        }

        if (options.UseRedis)
        {
            services.ConfigureRedis(options.Configuration);
            logger.LogInformation("Redis enabled...");
        }

        if(options.UseJwtAuthentication)
        {
            services.AddJwtAuthentication(options, logger);
            logger.LogInformation("Redis enabled...");
        }

        logger.LogInformation("Finished Adding Infrastructure Services, finalizing...");
        return services;
    }

    #region [ Helpers ]

    private static IServiceCollection ConfigureConsulServices(this IServiceCollection services, InfrastructureSetupOptions options)
    {
        string address = options.Configuration["ConsulConfiguration:Address"] 
            ?? throw new InvalidOperationException("ConsulConfiguration:Address is not defined!");

        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(address);
        }));
        services.AddSingleton<IHostedService, ConsulHostedService>();
        services.Configure<ConsulConfiguration>(options.Configuration.GetSection("consul"));
        return services;
    }

    private static IServiceCollection ConfigureDatabase<TContext>(this IServiceCollection services, InfrastructureSetupOptions options) 
        where TContext : DbContext
    {
        string? connectionString = options.Configuration.GetConnectionString("DefaultConnection");

        if(options.UseOutBoxMessageMechanism)
        {
            services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        }

        services.AddDbContext<TContext>((sp, opt) =>
        {
            opt.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(options.DbContextAssemblyName);
                if (options.UseOutBoxMessageMechanism)
                {
                    ConvertDomainEventsToOutboxMessagesInterceptor outboxMessageInterceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()
                        ?? throw new ArgumentException("ConvertDomainEventsToOutboxMessagesInterceptor was null!");

                    opt.UseNpgsql(connectionString).AddInterceptors(outboxMessageInterceptor);
                }
                else
                {
                    opt.UseNpgsql(connectionString);
                }
            });
        });
        return services;
    }

    private static IServiceCollection ConfigureOutBoxMessageJob<TContext>(this IServiceCollection services, InfrastructureSetupOptions options)
        where TContext : NoxDatabaseContext
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob<TContext>));
            configure
                .AddJob<ProcessOutboxMessagesJob<TContext>>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey).StartNow()
                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(options.OutBoxMessageJobIntervalSeconds).RepeatForever()));
        });

        services.AddQuartzHostedService();
        return services;
    }

    private static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        RedisConfiguration? redisConfig = configuration.GetSection("Redis").Get<RedisConfiguration>();
        if (redisConfig == null || string.IsNullOrWhiteSpace(redisConfig.ConnectionString))
        {
            throw new InvalidOperationException("Redis configuration is missing or invalid.");
        }

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig.ConnectionString));
        services.AddSingleton<ICacheService, RedisCacheService>();
        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, InfrastructureSetupOptions options, INoxLogger logger)
    {
        logger.LogInformation("Registering JWT Configuration.");
        JwtConfiguration jwtConfiguration = new();
        options.Configuration.GetSection("JwtSettings").Bind(jwtConfiguration);
        services.AddSingleton(jwtConfiguration);

        services.AddAuthentication(options =>
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

        services.AddAuthorization(opt =>
        {
            List<string> _claims = [.. options.Claims];
            List<string> _adminclaims = [.. options.AdminClaims];

            // Normal policies
            foreach (var item in _claims)
            {
                opt.AddPolicy(item, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(item, "API.Permission"));
                });
            }

            // Admin only policies
            foreach (var item in _adminclaims)
            {
                opt.AddPolicy($"{item}.Admin", builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(item, "API.Permission"))
                    .AddRequirements(new PermissionRequirement("admin", "role"));
                });
            }
        });

        if(options.RegisterIAuthorizationHandler)
        {
            services.AddScoped<IAuthorizationHandler, NoxJwtAuthorizationHandler>();
        }
        else
        {
            services.AddScoped<NoxJwtAuthorizationHandler>();
        }

        services.AddScoped<INoxTokenManager, NoxTokenManager>();
        logger.LogInformation("Registering JWT Configuration completed.");

        return services;
    }

    #endregion
}