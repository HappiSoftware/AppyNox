using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Logger;
using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Infrastructure.Logger;
using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using AppyNox.Services.Coupon.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void AddCouponApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.Load("AppyNox.Services.Coupon.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Coupon.Application"));

            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));
            services.AddScoped<INoxApplicationLogger, NoxApplicationLogger>();
        }

        #endregion
    }
}