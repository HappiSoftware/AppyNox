using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AutoMapper;
using CouponAggreagete = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Profiles;

public class CouponProfile : Profile
{
    #region [ Public Constructors ]

    public CouponProfile()
    {
        CreateMap<CouponCompositeCreateDto, CouponAggreagete>()
            .ConstructUsing((src, context) =>
            {
                return new CouponBuilder()
                    .WithDetails(src.Code, src.Description, string.Empty)
                    .WithAmount(context.Mapper.Map<Amount>(src.Amount))
                    .WithCouponDetail(context.Mapper.Map<CouponDetail>(src.CouponDetail))
                    .MarkAsBulkCreate()
                    .Build();
            });

        CreateMap<CouponCreateDto, CouponAggreagete>()
            .ConstructUsing((src, context) =>
            {
                return new CouponBuilder()
                    .WithDetails(src.Code, src.Description, string.Empty)
                    .WithAmount(context.Mapper.Map<Amount>(src.Amount))
                    .WithCouponDetailId(context.Mapper.Map<CouponDetailId>(src.CouponDetailId))
                    .Build();
            });

        CreateMap<CouponAggreagete, CouponDto>().MapAuditInformation();

        CreateMap<CouponId, CouponIdDto>().ReverseMap();

        CreateMap<Amount, AmountDto>().ReverseMap();
    }

    #endregion
}