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
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace AppyNox.Services.License.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static IHostApplicationBuilder AddLicenseInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration, INoxLogger logger)
        {
            IServiceCollection services = builder.Services;
            builder.AddInfrastructureServices<LicenseDatabaseContext>(logger, options =>
            {
                options.Assembly = Assembly.GetExecutingAssembly().GetName().Name;
                options.AspireDb = "appynox-license-db";
                options.UseOutBoxMessageMechanism = true;
                options.OutBoxMessageJobIntervalSeconds = 10;
                options.UseConsul = true;
                options.UseRedis = true;
                options.UseJwtAuthentication = true;
                options.AuthorizationSchemes =
                [
                    new()
                    {
                        Permissions = [.. Permissions.Licenses.Metrics, .. Permissions.Products.Metrics],
                    }
                ];
                options.Configuration = configuration;
                options.UseMassTransit = true;
                options.MassTransitConfiguration = busConfigurator =>
                {
                    busConfigurator.AddConsumer<ValidateLicenseMessageConsumer>();
                    busConfigurator.AddConsumer<AssignLicenseToUserMessageConsumer>();
                    busConfigurator.AddConsumer<GetLicenseIdByKeyDataRequestConsumer>();
                };
                options.RabbitMqConfiguration = rabbitMqConfiguration =>
                {
                    var (context, configurator) = rabbitMqConfiguration;
                    configurator.ReceiveEndpoint("validate-license", e =>
                    {
                        e.ConfigureConsumer<ValidateLicenseMessageConsumer>(context);
                    });

                    configurator.ReceiveEndpoint("assign-license-to-user", e =>
                    {
                        e.ConfigureConsumer<AssignLicenseToUserMessageConsumer>(context);
                    });

                    configurator.ReceiveEndpoint("get-license-by-key", e =>
                    {
                        e.ConfigureConsumer<GetLicenseIdByKeyDataRequestConsumer>(context);
                    });
                };
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(INoxRepository<>), typeof(NoxRepository<>));
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return builder;
        }

        #endregion
    }
}