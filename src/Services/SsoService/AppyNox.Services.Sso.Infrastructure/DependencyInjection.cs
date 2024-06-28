using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.MassTransit.Bus;
using AppyNox.Services.Base.Infrastructure.Authentication;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Sso.Application.Interfaces.Authentication;
using AppyNox.Services.Sso.Application.Permission;
using AppyNox.Services.Sso.Application.Validators.SharedRules;
using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Sso.Infrastructure.Authentication;
using AppyNox.Services.Sso.Infrastructure.Configuration;
using AppyNox.Services.Sso.Infrastructure.Data;
using AppyNox.Services.Sso.Infrastructure.Managers;
using AppyNox.Services.Sso.Infrastructure.MassTransit.Consumers;
using AppyNox.Services.Sso.Infrastructure.MassTransit.Filters;
using AppyNox.Services.Sso.Infrastructure.MassTransit.Sagas;
using AppyNox.Services.Sso.Infrastructure.Services;
using AppyNox.Services.Sso.SharedEvents.Events;
using Consul;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Reflection;

namespace AppyNox.Services.Sso.Infrastructure;

/// <summary>
/// Provides extension methods for IServiceCollection to register sso infrastructure.
/// </summary>
public static class DependencyInjection
{
    #region [ Public Methods ]

    /// <summary>
    /// Adds sso infrastructure services, including database configuration and Consul Discovery Service.
    /// </summary>
    /// <param name="builder">The IHostApplicationBuilder of the program.</param>
    /// <param name="configuration">The IConfiguration instance to access application settings.</param>
    public static IServiceCollection AddSsoInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        INoxLogger noxLogger,
        bool isWeb = false
    )
    {

        services.AddSingleton<INoxInfrastructureLogger, NoxInfrastructureLogger>();

        #region [ Database Configuration ]

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IdentityDatabaseContext>(
            options => options.UseNpgsql(connectionString)
        );

        #endregion

        #region [ Consul Discovery Service ]

        services.AddSingleton<IConsulClient, ConsulClient>(
            p =>
                new ConsulClient(consulConfig =>
                {
                    var address =
                        configuration["ConsulConfiguration:Address"] ?? "http://localhost:8500";
                    consulConfig.Address = new Uri(address);
                })
        );
        services.AddSingleton<IHostedService, ConsulHostedService>();
        services.Configure<ConsulConfiguration>(configuration.GetSection("consul"));

        #endregion

        #region [ MassTransit ]

        // Add to DI to be able to migrate changes
        services.AddDbContext<IdentitySagaDatabaseContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("SagaConnection")),
            ServiceLifetime.Scoped
        );

        services.AddMassTransit(busConfigurator =>
        {
            #region [ Consumers ]

            busConfigurator.AddConsumer<CreateApplicationUserMessageConsumer>();
            busConfigurator.AddConsumer<DeleteApplicationUserMessageConsumer>();

            #endregion

            #region [ StateMachine ]

            busConfigurator
                .AddSagaStateMachine<UserCreationSaga, UserCreationSagaState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.AddDbContext<DbContext, IdentitySagaDatabaseContext>(
                        (provider, builder) =>
                        {
                            builder.UseNpgsql(
                                configuration.GetConnectionString("SagaConnection")
                            );
                        }
                    );
                    r.UsePostgres();
                });

            #endregion

            #region [ RabbitMQ ]

            busConfigurator.UsingRabbitMq(
                (context, configurator) =>
                {
                    configurator.Host(
                        new Uri(configuration["MessageBroker:Host"]!),
                        h =>
                        {
                            h.Username(configuration["MessageBroker:Username"]!);
                            h.Password(configuration["MessageBroker:Password"]!);
                        }
                    );

                    // Apply filters to the send and publish pipelines
                    configurator.ConfigureSend(sendConfig =>
                    {
                        sendConfig.UseFilter(new AddSsoContextToSendContextFilter());
                    });

                    configurator.ConfigurePublish(publishConfig =>
                    {
                        publishConfig.UseFilter(new AddSsoContextToSendContextFilter());
                    });


                    #region [ Endpoints ]

                    configurator.ReceiveEndpoint(
                        "create-user",
                        e =>
                        {
                            e.ConfigureConsumer<CreateApplicationUserMessageConsumer>(context);
                            e.UseConsumeFilter<SsoContextConsumeFilter<CreateApplicationUserMessageConsumer>>(context);
                        }
                    );
                    configurator.ReceiveEndpoint(
                        "delete-user",
                        e =>
                        {
                            e.ConfigureConsumer<DeleteApplicationUserMessageConsumer>(context);
                            e.UseConsumeFilter<SsoContextConsumeFilter<DeleteApplicationUserMessage>>(context);
                        }
                    );

                    #endregion

                    configurator.ConfigureEndpoints(context);
                }
            );

            #endregion
        });

        #region [INoxBus Configuration]

        services.AddMassTransit<INoxBus>(busConfigurator =>
        {
            #region [ Consumers ]

            busConfigurator.AddConsumer<CheckUserAvailabilityMessageConsumer>();

            #endregion

            #region [ RabbitMQ ]

            busConfigurator.UsingRabbitMq(
                (context, configurator) =>
                {
                    configurator.Host(
                        new Uri(configuration["NoxMessageBroker:Host"]!),
                        h =>
                        {
                            h.Username(configuration["NoxMessageBroker:Username"]!);
                            h.Password(configuration["NoxMessageBroker:Password"]!);
                        }
                    );

                    // Apply filters to the send and publish pipelines
                    configurator.ConfigureSend(sendConfig =>
                    {
                        sendConfig.UseFilter(new AddSsoContextToSendContextFilter());
                    });

                    configurator.ConfigurePublish(publishConfig =>
                    {
                        publishConfig.UseFilter(new AddSsoContextToSendContextFilter());
                    });

                    #region [ Endpoints ]

                    configurator.ReceiveEndpoint("check-user-availability-in-sso", e =>
                    {
                        e.ConfigureConsumer<CheckUserAvailabilityMessageConsumer>(context);
                        e.UseConsumeFilter<SsoContextConsumeFilter<CheckUserAvailabilityMessageConsumer>>(context);
                    });

                    #endregion

                    configurator.ConfigureEndpoints(context);
                }
            );

            #endregion
        });

        #endregion

        #endregion

        #region [ Identity ]

        if (!isWeb)
        {
            services.AddJwtAuthenticationAndAuthorization(configuration, noxLogger);
        }
        else
        {
            services.AddCookieAuthenticationAndAuthorization();
        }

        services.AddScoped<SignInManager<ApplicationUser>>();
        services.AddScoped<UserManager<ApplicationUser>>();
        services.AddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
        services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, ApplicationRole, IdentityDatabaseContext, Guid>>();
        services.AddScoped<ICustomTokenManager, JwtTokenManager>();
        services.AddScoped<ICustomUserManager, CustomUserManager>();

        #endregion

        services.AddScoped<IDatabaseChecks, ValidatorDatabaseChecker>();

        return services;
    }

    #endregion

    private static void AddJwtAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration, INoxLogger noxLogger)
    {
        noxLogger.LogInformation("Registering JWT Configuration.");

        services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddSignInManager()
               .AddEntityFrameworkStores<IdentityDatabaseContext>()
               .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        });
        var jwtConfiguration = new SsoJwtConfiguration();
        configuration.GetSection("JwtSettings:AppyNox").Bind(jwtConfiguration);

        // Add JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "NoxSsoJwtScheme";
            options.DefaultChallengeScheme = "NoxSsoJwtScheme";
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
        .AddScheme<AuthenticationSchemeOptions, NoxSsoJwtAuthenticationHandler>("NoxSsoJwtScheme", options => { });

        // Add Policy-based Authorization
        services.AddAuthorization(options =>
        {
            List<string> _claims = [.. Permissions.Users.Metrics, .. Permissions.Roles.Metrics];

            foreach (var item in _claims)
            {
                options.AddPolicy(item, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(item, "API.Permission"));
                });
            }
        });

        services.AddScoped<IAuthorizationHandler, NoxSsoAuthorizationHandler>();
        noxLogger.LogInformation("Registering JWT Configuration completed.");
    }

    private static void AddCookieAuthenticationAndAuthorization(this IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDatabaseContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            // Default SignIn settings.
            //options.SignIn.RequireConfirmedEmail = true;
            //options.SignIn.RequireConfirmedPhoneNumber = false;
            //options.SignIn.RequireConfirmedAccount = true;

            //// User settings
            //options.User.RequireUniqueEmail = true;
        });

        services
            .AddAuthorizationCore(options =>
            {
                List<string> _claims = [.. Permissions.Users.Metrics, .. Permissions.Roles.Metrics];

                foreach (var item in _claims)
                {
                    options.AddPolicy(item, builder =>
                    {
                        builder.AddRequirements(new PermissionRequirement(item, "API.Permission"));
                    });
                }
            })
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(options => { });


        services.ConfigureApplicationCookie(options => { options.LoginPath = "/pages/authentication/login"; });
    }

}

