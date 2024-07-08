using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Infrastructure;
using AppyNox.Services.Coupon.Application.Permission;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AppyNox.Services.License.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.Services.Coupon.Infrastructure;

public static class DependencyInjection
{
    #region [ Public Methods ]

    /// <summary>
    /// Centralized Dependency Injection For Infrastructure Layer.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static IServiceCollection AddCouponInfrastructure(this IServiceCollection services, IConfiguration configuration, INoxLogger logger)
    {
        services.AddInfrastructureServices<CouponDbContext>(logger, options =>
        {
            options.Assembly = "AppyNox.Services.Coupon.Infrastructure";
            options.UseOutBoxMessageMechanism = true;
            options.OutBoxMessageJobIntervalSeconds = 10;
            options.UseEncryption = true;
            options.UseConsul = true;
            options.UseRedis = true;
            options.UseJwtAuthentication = true;
            options.AuthorizationSchemes =
            [
                new()
                {
                    Permissions = [.. Permissions.Coupons.Metrics],
                },
                new()
                {
                    IsAdmin = true,
                    Permissions = [.. Permissions.CouponsAdmin.Metrics]
                }
            ];
            options.Configuration = configuration;
            options.UseMassTransit = true;
        });

        services.AddScoped(typeof(ICouponRepository), typeof(CouponRepository));
        services.AddScoped(typeof(INoxRepository<>), typeof(NoxRepository<>));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ILicenseServiceClient, LicenseServiceClient>();

        return services;
    }

    #endregion
}