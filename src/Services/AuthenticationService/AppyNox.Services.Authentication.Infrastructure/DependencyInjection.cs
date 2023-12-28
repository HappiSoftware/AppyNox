using AppyNox.Services.Authentication.Infrastructure.Data;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Logger;
using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppyNox.Services.Authentication.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void AddAuthenticationInfrastructure(IServiceCollection services, IConfiguration configuration, ApplicationEnvironment environment)
        {
            services.AddScoped<INoxInfrastructureLogger, NoxInfrastructureLogger>();

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

            //services.AddHostedService<DatabaseStartupHostedService<IdentityDbContext>>();
        }

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var _db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }

        #endregion
    }
}