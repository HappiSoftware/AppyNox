﻿using AppyNox.Services.Coupon.Application.DTOUtilities;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using AppyNox.Services.Coupon.Application.Services.Interfaces;
using AppyNox.Services.Coupon.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.Load("AppyNox.Services.Coupon.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Coupon.Application"));

            services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
            services.AddSingleton<DTOMappingRegistry>();
        }
    }
}
