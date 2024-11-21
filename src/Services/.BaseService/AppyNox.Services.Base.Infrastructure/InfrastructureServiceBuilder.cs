using AppyNox.Services.Base.Application.Interfaces.Authentication;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Authentication;
using AppyNox.Services.Base.Infrastructure.BackgroundJobs;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace AppyNox.Services.Base.Infrastructure;

public class InfrastructureSetupOptions
{
#nullable disable
    [Required]
    public string Assembly { get; set; }
    [Required]
    public IConfiguration Configuration { get; set; }
    [Required]
    public string AspireDb { get; set; }
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
    public static IHostApplicationBuilder AddInfrastructureServices<TContext>(
        this IHostApplicationBuilder builder,
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
        IServiceCollection services = builder.Services;

        services.AddSingleton(typeof(INoxApplicationLogger<>), typeof(NoxApplicationLogger<>));
        services.AddSingleton(typeof(INoxInfrastructureLogger<>), typeof(NoxInfrastructureLogger<>));
        services.AddSingleton(typeof(INoxApiLogger<>), typeof(NoxApiLogger<>));
        logger.LogInformation($"-{serviceName}- Loggers enabled...", false);

        if (options.UseConsul)
        {
            services.ConfigureConsulServices(options);
            logger.LogInformation($"-{serviceName}- Consul enabled...", false);
        }

        builder.ConfigureDatabase<TContext>(options);
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
            builder.ConfigureRedis();
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
        return builder;
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

    private static IHostApplicationBuilder ConfigureDatabase<TContext>(this IHostApplicationBuilder builder, InfrastructureSetupOptions options)
        where TContext : DbContext, INoxDatabaseContext
    {
        if (options.UseOutBoxMessageMechanism)
        {
            builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        }

        builder.AddNpgsqlDbContext<TContext>(
        options.AspireDb,
        configureDbContextOptions: opt =>
        {
            opt.UseNpgsql(npgsqlOptions =>
            {
                opt.UseLazyLoadingProxies();
                npgsqlOptions.MigrationsAssembly(options.Assembly);

                if (options.UseOutBoxMessageMechanism)
                {
                    var outboxMessageInterceptor = builder.Services.BuildServiceProvider()
                        .GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()
                        ?? throw new ArgumentException("ConvertDomainEventsToOutboxMessagesInterceptor was null!");

                    opt.AddInterceptors(outboxMessageInterceptor);
                }
            });
        });

        builder.EnrichNpgsqlDbContext<TContext>();

        return builder;
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

    private static IServiceCollection ConfigureRedis(this IHostApplicationBuilder builder)
    {
        builder.AddRedisClient("appynox-cache");
        builder.Services.AddSingleton<ICacheService, RedisCacheService>();
        return builder.Services;
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

                var configService = context.GetRequiredService<IConfiguration>();
                var connectionString = configService.GetConnectionString("appynox-rabbitmq");
                configurator.Host(connectionString);

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