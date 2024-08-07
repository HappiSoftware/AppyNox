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
using AppyNox.Services.Base.Infrastructure.MassTransit.Filters;
using AppyNox.Services.Base.Infrastructure.Services;
using AppyNox.Services.Base.Infrastructure.Services.CacheServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Consul;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Serilog;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace AppyNox.Services.Base.Infrastructure;

public class InfrastructureSetupOptions
{
#nullable disable
    [Required]
    public string Assembly { get; set; }
    [Required]
    public IConfiguration Configuration { get; set; }
    public bool UseOutBoxMessageMechanism { get; set; } = false;
    public int OutBoxMessageJobIntervalSeconds { get; set; } = 30;
    public bool UseEncryption { get; set; } = false;
    public bool UseConsul { get; set; } = false;
    public bool UseRedis { get; set; } = false;
    public bool UseJwtAuthentication { get; set; } = true;
    public string JwtConfigurationPath { get; set; } = "JwtSettings";
    public bool UseDefaultAuthenticationScheme { get; set; } = true;
    public List<AuthorizationSchemeBundle> AuthorizationSchemes { get; set; }
    public bool UseMassTransit { get; set; } = false;
    public Action<IBusRegistrationConfigurator> MassTransitConfiguration { get; set; }
    public Action<(IBusRegistrationContext, IRabbitMqBusFactoryConfigurator)> RabbitMqConfiguration { get; set; }
#nullable enable
}


public static class InfrastructureServiceBuilder
{
    public static IServiceCollection AddInfrastructureServices<TContext>(
        this IServiceCollection services,
        INoxLogger logger,
        Action<InfrastructureSetupOptions> configureOptions) where TContext : DbContext, INoxDatabaseContext
    {
        var options = new InfrastructureSetupOptions();
        configureOptions(options);
        string serviceName = options.Configuration["Consul:ServiceName"]
            ?? throw new InvalidOperationException("Consul:ServiceName is not defined!");

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);
        if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidOperationException($"Invalid options: {errors}");
        }

        services.AddSingleton(typeof(INoxApplicationLogger<>), typeof(NoxApplicationLogger<>));
        services.AddSingleton(typeof(INoxInfrastructureLogger<>), typeof(NoxInfrastructureLogger<>));
        services.AddSingleton(typeof(INoxApiLogger<>), typeof(NoxApiLogger<>));
        logger.LogInformation($"-{serviceName}- Loggers enabled...", false);

        if (options.UseConsul)
        {
            services.ConfigureConsulServices(options);
            logger.LogInformation($"-{serviceName}- Consul enabled...", false);
        }

        services.ConfigureDatabase<TContext>(options);
        logger.LogInformation($"-{serviceName}- Database connection enabled...", false);

        if (options.UseEncryption)
        {
            services.AddSingleton<IEncryptionService, EncryptionService>();
            logger.LogInformation($"-{serviceName}- Encryption enabled...", false);
        }

        if (options.UseOutBoxMessageMechanism)
        {
            services.ConfigureOutBoxMessageJob<TContext>(options);
            logger.LogInformation($"-{serviceName}- OutBoxMessageJob enabled...", false);
        }

        if (options.UseRedis)
        {
            services.ConfigureRedis(options.Configuration);
            logger.LogInformation($"-{serviceName}- Redis enabled...", false);
        }

        if (options.UseJwtAuthentication)
        {
            services.AddJwtAuthentication(serviceName, options, logger);
            logger.LogInformation($"-{serviceName}- Jwt Authentication enabled...", false);
        }

        if (options.UseMassTransit)
        {
            services.AddMassTransit(serviceName, options, logger);
        }

        logger.LogInformation($"-{serviceName}- Finished Adding Infrastructure Services, finalizing...", false);
        return services;
    }

    #region [ Builders ]

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
        where TContext : DbContext, INoxDatabaseContext
    {
        string? connectionString = options.Configuration.GetConnectionString("DefaultConnection");

        if (options.UseOutBoxMessageMechanism)
        {
            services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        }

        services.AddDbContext<TContext>((sp, opt) =>
        {
            opt.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(options.Assembly);
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
        where TContext : DbContext, INoxDatabaseContext
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

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string serviceName, InfrastructureSetupOptions options, INoxLogger logger)
    {
        logger.LogInformation($"-{serviceName}- Registering JWT Configuration.", false);
        JwtConfiguration jwtConfiguration = new();
        options.Configuration.GetSection(options.JwtConfigurationPath).Bind(jwtConfiguration);
        services.AddSingleton(jwtConfiguration);

        services.AddAuthentication(opt =>
        {
            if(options.UseDefaultAuthenticationScheme)
            {
                opt.DefaultAuthenticateScheme = "NoxJwtScheme";
                opt.DefaultChallengeScheme = "NoxJwtScheme";
            }
        })
        .AddScheme<AuthenticationSchemeOptions, NoxJwtAuthenticationHandler>("NoxJwtScheme", options =>
        {
        });

        services.AddAuthorization(options);

        services.AddScoped<IAuthorizationHandler, NoxJwtAuthorizationHandler>();
        services.AddScoped<INoxTokenManager, NoxTokenManager>();
        logger.LogInformation($"-{serviceName}- Registering JWT Configuration completed.", false);

        return services;
    }

    private static IServiceCollection AddMassTransit(this IServiceCollection services, string serviceName, InfrastructureSetupOptions options, INoxLogger logger)
    {
        string hostUrl = options.Configuration["MessageBroker:Host"]
                ?? throw new InvalidOperationException("MessageBroker:Host is not defined!");

        string username = options.Configuration["MessageBroker:Username"]
            ?? throw new InvalidOperationException("MessageBroker:Username is not defined!");
        string password = options.Configuration["MessageBroker:Password"]
            ?? throw new InvalidOperationException("MessageBroker:Password is not defined!");

        services.AddMassTransit(busConfigurator =>
        {
            options.MassTransitConfiguration?.Invoke(busConfigurator);

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.ConfigureSend(sendConfig =>
                {
                    sendConfig.UseFilter(new NoxContextFilter());
                });

                configurator.ConfigurePublish(publishConfig =>
                {
                    publishConfig.UseFilter(new NoxContextFilter());
                });

                configurator.UseConsumeFilter(typeof(NoxContextConsumeFilter<>), context);

                configurator.Host(
                        new Uri(hostUrl),
                        h =>
                        {
                            h.Username(username);
                            h.Password(password);
                        }
                    );

                options.RabbitMqConfiguration?.Invoke((context, configurator));

                configurator.ConfigureEndpoints(context);
            });
        });

        logger.LogInformation($"-{serviceName}- MassTransit configured with RabbitMQ.", false);
        return services;
    }
    #endregion

    #region [ Helpers ]

    private static IServiceCollection AddAuthorization(this IServiceCollection services, InfrastructureSetupOptions options)
    {
        services.AddAuthorizationCore(opt =>
        {
            foreach (var scheme in options.AuthorizationSchemes)
            {
                foreach(var claim in scheme.Permissions)
                {
                    opt.AddPolicy(claim, builder =>
                    {
                        builder
                        .AddAuthenticationSchemes(scheme.SchemeToAdd)
                        .RequireAuthenticatedUser()
                        .AddRequirements(new PermissionRequirement(claim, "API.Permission"));
                        if (scheme.IsAdmin)
                        {
                            builder.AddRequirements(new PermissionRequirement("admin", "role"));
                        }
                    });
                }
            }
        });
        return services;
    }

    #endregion
}

#region [ Options Classes ]

public class AuthorizationSchemeBundle
{
    public bool IsAdmin { get; set; } = false;
    public List<string> Permissions { get; set; } = [];
    public string PermissionType { get; set; } = "API.Permission";
    public string SchemeToAdd { get; set; } = "NoxJwtScheme";
}

#endregion