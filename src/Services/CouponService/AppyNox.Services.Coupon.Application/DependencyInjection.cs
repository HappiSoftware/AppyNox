﻿using AppyNox.Services.Base.Application;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Coupon.Application.DtoUtilities;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application;

public static class DependencyInjection
{
    #region [ Public Methods ]

    public static IServiceCollection AddCouponApplication(this IServiceCollection services, IConfiguration configuration, INoxLogger logger)
    {
        services.AddApplicationServices(logger, options =>
        {
            options.Assembly = Assembly.GetExecutingAssembly().GetName().Name;
            options.Configuration = configuration;
            options.UseAutoMapper = true;
            options.UseFluentValidation = true;
            options.UseDtoMappingRegistry = true;
            options.DtoMappingRegistryFactory = provider => new DtoMappingRegistry();
            options.UseMediatR = true;
        });

        services.AddNoxEntityCommands<Domain.Coupons.Coupon, CouponId>();
        services.AddAnemicEntityCommands<Ticket>();

        return services;
    }

    #endregion
}