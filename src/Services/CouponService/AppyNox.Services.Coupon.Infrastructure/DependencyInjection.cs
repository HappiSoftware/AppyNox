using AppyNox.Services.Coupon.Domain.Interfaces;
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

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CouponDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<CouponDbContext>();

                //if (_db.Database.GetPendingMigrations().Any())
                //{
                //    _db.Database.Migrate();
                //}
            }
        }

        #endregion
    }
}