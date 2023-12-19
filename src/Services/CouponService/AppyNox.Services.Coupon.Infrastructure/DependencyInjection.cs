using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.Services.Coupon.Infrastructure
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
            services.AddDbContext<CouponDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped(typeof(IGenericRepositoryBase<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWorkBase, UnitOfWork>();
        }

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var _db = scope.ServiceProvider.GetRequiredService<CouponDbContext>();

            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }

        #endregion
    }
}