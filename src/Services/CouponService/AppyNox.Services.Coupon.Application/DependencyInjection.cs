﻿using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.Load("AppyNox.Services.Coupon.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Coupon.Application"));

            services.AddScoped(typeof(IGenericServiceBase<,,,>), typeof(GenericService<,,,>));
            services.AddSingleton<DtoMappingRegistry>();
        }

        #endregion
    }
}