using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Infrastructure;
using AppyNox.Services.Coupon.Application.Permission;
using AppyNox.Services.Coupon.Infrastructure.Authentication;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AppyNox.Services.License.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace AppyNox.Services.Coupon.Infrastructure;

public static class DependencyInjection
{
    #region [ Public Methods ]

    /// <summary>
    /// Centralized Dependency Injection For Infrastructure Layer.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddCouponInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration, INoxLogger logger)
    {
        IServiceCollection services = builder.Services;
        builder.AddInfrastructureServices<CouponDbContext>(logger, options =>
        {
            options.Assembly = Assembly.GetExecutingAssembly().GetName().Name;
            options.AspireDb = "appynox-coupon-db";
            options.UseOutBoxMessageMechanism = true;
            options.OutBoxMessageJobIntervalSeconds = 10;
            options.UseEncryption = true;
            options.UseConsul = true;
            options.UseRedis = true;
            options.UseJwtAuthentication = true;
            options.UseDefaultAuthenticationScheme = false;
            options.JwtConfigurationPath = "JwtSettings:Sso";
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
                },
                new()
                {
                    Permissions = [.. Permissions.CouponCustomJwt.Metrics],
                    SchemeToAdd = "CouponJwtScheme"
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

        // Custom Authentication implementation
        CouponTokenConfiguration couponJwtConfiguration = new();
        configuration.GetSection("JwtSettings:Coupon").Bind(couponJwtConfiguration);
        services.AddSingleton(couponJwtConfiguration);

        services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, CouponAuthenticationHandler>("CouponJwtScheme", null);

        services.AddScoped<ICouponTokenManager, CouponTokenManager>();
        logger.LogInformation("Registered Fleet JWT Configuration.", false);

        return builder;
    }

    #endregion
}