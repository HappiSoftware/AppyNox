using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;
using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppyNox.Services.License.Application.Interfaces;
using System.Reflection;
using FluentValidation;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.License.Application.Dtos.DtoUtilities;
using AppyNox.Services.Base.Application.Helpers;

namespace AppyNox.Services.License.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        /// <summary>
        /// Centralized Dependency Injection For Infrastructure Layer.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IServiceCollection AddLicenseInfrastructure(this IServiceCollection services, IHostApplicationBuilder builder, ApplicationEnvironment environment, INoxLogger logger)
        {
            IConfiguration configuration = builder.Configuration;
            string environmentName = builder.Environment.EnvironmentName;

            services.AddSingleton<INoxInfrastructureLogger, NoxInfrastructureLogger>();
            services.AddSingleton<INoxApplicationLogger, NoxApplicationLogger>();

            #region [ Database Configuration ]

            string? connectionString = string.Empty;
            connectionString = environment switch
            {
                ApplicationEnvironment.Development => configuration.GetConnectionString("DevelopmentConnection"),
                ApplicationEnvironment.Staging => configuration.GetConnectionString("StagingConnection"),
                ApplicationEnvironment.Production => configuration.GetConnectionString("ProductionConnection"),
                _ => configuration.GetConnectionString("DefaultConnection"),
            };

            services.AddDbContext<LicenseDatabaseContext>(options =>
                options.UseNpgsql(connectionString));

            logger.LogInformation($"Connection String: {connectionString}");

            #endregion

            #region [ Consul Discovery Service ]

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration["ConsulConfig:Address"] ?? "http://localhost:8500";
                consulConfig.Address = new Uri(address);
            }));
            services.AddSingleton<IHostedService, ConsulHostedService>();
            services.Configure<ConsulConfig>(configuration.GetSection("consul"));

            #endregion

            services.AddScoped(typeof(IGenericRepositoryBase<>), typeof(GenericRepository<>));
            services.AddScoped<ILicenseRepository, LicenseRepository<LicenseEntity>>();
            services.AddScoped<IUnitOfWorkBase, UnitOfWork>();

            #region [ Application ]

            Assembly applicationAssembly = Assembly.Load("AppyNox.Services.License.Application");
            services.AddAutoMapper(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);

            #region [ CQRS ]

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });
            services.AddGenericEntityCommandHandlers(typeof(LicenseEntity));

            #endregion

            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));

            #endregion

            return services;
        }

        #endregion
    }
}