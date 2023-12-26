using AppyNox.Services.Authentication.Infrastructure.Data;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.Services.Authentication.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, ApplicationEnvironment environment)
        {
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
            services.AddHostedService<DatabaseStartupHostedService<IdentityDbContext>>();
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