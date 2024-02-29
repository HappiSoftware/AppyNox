using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Coupon.Application.DtoUtilities;
using AppyNox.Services.Coupon.Domain.Coupons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void AddCouponApplication(this IServiceCollection services)
        {
            Assembly applicationAssembly = Assembly.Load("AppyNox.Services.Coupon.Application");
            services.AddAutoMapper(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);

            #region [ CQRS ]

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });

            services.AddNoxEntityCommands<Domain.Coupons.Coupon, CouponId>();

            #endregion

            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));
        }

        #endregion
    }
}