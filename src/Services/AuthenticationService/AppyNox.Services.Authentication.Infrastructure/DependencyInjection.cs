using AppyNox.Services.Authentication.Application.Validators.SharedRules;
using AppyNox.Services.Authentication.Infrastructure.Data;
using AppyNox.Services.Authentication.Infrastructure.Services;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.License.Application.Dtos.DtoUtilities;
using Consul;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace AppyNox.Services.Authentication.Infrastructure
{
    /// <summary>
    /// Provides extension methods for IServiceCollection to register authentication infrastructure.
    /// </summary>
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        /// <summary>
        /// Adds authentication infrastructure services, including database configuration and Consul Discovery Service.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration instance to access application settings.</param>
        /// <param name="environment">The current application environment.</param>
        public static void AddAuthenticationInfrastructure(this IServiceCollection services, IConfiguration configuration, ApplicationEnvironment environment)
        {
            services.AddSingleton<INoxInfrastructureLogger, NoxInfrastructureLogger>();

            #region [ Database Configuration ]

            string? connectionString = string.Empty;
            connectionString = environment switch
            {
                ApplicationEnvironment.Development => configuration.GetConnectionString("DevelopmentConnection"),
                ApplicationEnvironment.Staging => configuration.GetConnectionString("StagingConnection"),
                ApplicationEnvironment.Production => configuration.GetConnectionString("ProductionConnection"),
                _ => configuration.GetConnectionString("DefaultConnection"),
            };

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(connectionString));

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

            #region [ Application ]

            Assembly applicationAssembly = Assembly.Load("AppyNox.Services.Authentication.Application");
            services.AddAutoMapper(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);

            #region [ CQRS ]

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });

            #endregion

            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));

            #endregion

            services.AddScoped<IDatabaseChecks, ValidatorDatabaseChecker>();
        }

        #endregion
    }
}