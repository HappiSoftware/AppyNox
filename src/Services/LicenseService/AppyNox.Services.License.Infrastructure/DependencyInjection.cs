using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Infrastructure;
using AppyNox.Services.License.Application.Interfaces;
using AppyNox.Services.License.Application.Permission;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.MassTransit.Consumers;
using AppyNox.Services.License.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.Services.License.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static IServiceCollection AddLicenseInfrastructure(this IServiceCollection services, IConfiguration configuration, INoxLogger logger)
        {
            services.AddInfrastructureServices<LicenseDatabaseContext>(logger, options =>
            {
                options.DbContextAssemblyName = "AppyNox.Services.License.Infrastructure";
                options.UseOutBoxMessageMechanism = true;
                options.OutBoxMessageJobIntervalSeconds = 10;
                options.UseConsul = true;
                options.UseRedis = true;
                options.UseJwtAuthentication = true;
                options.RegisterIAuthorizationHandler = true;
                options.Claims = [.. Permissions.Licenses.Metrics, .. Permissions.Products.Metrics];
                options.Configuration = configuration;
            });

            #region [ MassTransit ]

            string hostUrl = configuration["MessageBroker:Host"]
                ?? throw new InvalidOperationException("MessageBroker:Host is not defined!");

            string username = configuration["MessageBroker:Username"]
                ?? throw new InvalidOperationException("MessageBroker:Username is not defined!");
            string password = configuration["MessageBroker:Password"]
                ?? throw new InvalidOperationException("MessageBroker:Password is not defined!");

            services.AddMassTransit(busConfigurator =>
            {
                #region [ Consumers ]

                busConfigurator.AddConsumer<ValidateLicenseMessageConsumer>();
                busConfigurator.AddConsumer<AssignLicenseToUserMessageConsumer>();

                #endregion

                #region [ RabbitMQ ]


                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(
                        new Uri(hostUrl),
                        h =>
                        {
                            h.Username(username);
                            h.Password(password);
                        }
                    );
                    configurator.ReceiveEndpoint("validate-license", e =>
                    {
                        e.ConfigureConsumer<ValidateLicenseMessageConsumer>(context);
                    });

                    configurator.ReceiveEndpoint("assign-license-to-user", e =>
                    {
                        e.ConfigureConsumer<AssignLicenseToUserMessageConsumer>(context);
                    });

                    configurator.ConfigureEndpoints(context);
                });

                #endregion
            });

            #endregion

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(INoxRepository<>), typeof(NoxRepository<>));
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        #endregion
    }
}