using AppyNox.Services.Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.Services.Authentication.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
        }

        #endregion
    }
}