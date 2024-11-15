using AppyNox.Services.Base.Application;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Entities;
using MediatR;
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
            options.UseMediatR = true;
        });

        services.AddNoxEntityCommands<Domain.Coupons.Coupon, CouponId, CouponCreateDto, CouponDto>();
        services.AddNoxEntityCompositeCreateCommand<Domain.Coupons.Coupon, CouponId, CouponCompositeCreateDto, CouponDto>();

        services.AddAnemicEntityCommands<Ticket, TicketCreateDto, TicketDto, TicketUpdateDto>();

        return services;
    }

    #endregion
}