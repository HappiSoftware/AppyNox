﻿using AppyNox.Services.Coupon.Application.DtoUtilities;
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

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.Load("AppyNox.Services.Coupon.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Coupon.Application"));

            services.AddScoped(typeof(IGenericService<,,>), typeof(GenericService<,,,>));
            services.AddSingleton<DtoMappingRegistry>();
        }

        #endregion
    }
}