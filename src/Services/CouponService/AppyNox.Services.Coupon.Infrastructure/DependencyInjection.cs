using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure;
using AppyNox.Services.Base.Infrastructure.Helpers;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Coupon.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static IServiceCollection AddCouponInfrastructure(this IServiceCollection services, IConfiguration configuration, ApplicationEnvironment environment, ILogger logger)
        {
            string? connectionString = string.Empty;
            connectionString = environment switch
            {
                ApplicationEnvironment.Development => configuration.GetConnectionString("DevelopmentConnection"),
                ApplicationEnvironment.Staging => configuration.GetConnectionString("StagingConnection"),
                ApplicationEnvironment.Production => configuration.GetConnectionString("ProductionConnection"),
                _ => configuration.GetConnectionString("DefaultConnection"),
            };

            logger.LogInformation("{Message}", $"Infrastructure is starting... Connection string: {connectionString}");

            services.AddDbContext<CouponDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddHostedService<DatabaseStartupHostedService<CouponDbContext>>();

            services.AddScoped(typeof(IGenericRepositoryBase<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWorkBase, UnitOfWork>();
            return services;
        }

        #endregion
    }
}