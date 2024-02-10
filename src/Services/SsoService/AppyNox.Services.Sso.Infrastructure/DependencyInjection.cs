using AppyNox.Services.Sso.Application.Interfaces;
using AppyNox.Services.Sso.Application.Validators.SharedRules;
using AppyNox.Services.Sso.Infrastructure.AsyncLocals;
using AppyNox.Services.Sso.Infrastructure.Data;
using AppyNox.Services.Sso.Infrastructure.MassTransit.Consumers;
using AppyNox.Services.Sso.Infrastructure.MassTransit.Sagas;
using AppyNox.Services.Sso.Infrastructure.Services;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Consul;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppyNox.Services.Sso.Infrastructure
{
    /// <summary>
    /// Provides extension methods for IServiceCollection to register sso infrastructure.
    /// </summary>
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        /// <summary>
        /// Adds sso infrastructure services, including database configuration and Consul Discovery Service.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration instance to access application settings.</param>
        /// <param name="environment">The current application environment.</param>
        public static void AddSsoInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            ApplicationEnvironment environment
        )
        {
            services.AddSingleton<INoxInfrastructureLogger, NoxInfrastructureLogger>();

            #region [ Database Configuration ]

            string? connectionString = string.Empty;
            connectionString = environment switch
            {
                ApplicationEnvironment.Development
                    => configuration.GetConnectionString("DevelopmentConnection"),
                ApplicationEnvironment.Staging
                    => configuration.GetConnectionString("StagingConnection"),
                ApplicationEnvironment.Production
                    => configuration.GetConnectionString("ProductionConnection"),
                _ => configuration.GetConnectionString("DefaultConnection"),
            };

            services.AddDbContext<IdentityDatabaseContext>(
                options => options.UseNpgsql(connectionString)
            );

            services.AddHttpContextAccessor();

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

                        #region [ Endpoints ]

                        configurator.ReceiveEndpoint(
                            "create-user",
                            e =>
                            {
                                e.ConfigureConsumer<CreateApplicationUserMessageConsumer>(context);
                            }
                        );
                        configurator.ReceiveEndpoint(
                            "delete-user",
                            e =>
                            {
                                e.ConfigureConsumer<DeleteApplicationUserMessageConsumer>(context);
                            }
                        );

                        #endregion

                        configurator.ConfigureEndpoints(context);
                    }
                );

                #endregion
            });

            #endregion

            services.AddScoped<IDatabaseChecks, ValidatorDatabaseChecker>();
        }

        #endregion
    }
}
