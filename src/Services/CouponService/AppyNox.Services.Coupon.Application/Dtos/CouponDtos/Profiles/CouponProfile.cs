using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
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
        CreateMap<CouponBulkCreateDto, CouponAggreagete>()
            .ConstructUsing((src, context) =>
            {
                return new CouponBuilder()
                    .WithDetails(src.Code, src.Description, string.Empty)
                    .WithAmount(context.Mapper.Map<Amount>(src.Amount))
                    .WithCouponDetail(context.Mapper.Map<CouponDetail>(src.CouponDetail))
                    .MarkAsBulkCreate()
                    .Build();
            });

        CreateMap<CouponSimpleCreateDto, CouponAggreagete>()
            .ConstructUsing((src, context) =>
            {
                return new CouponBuilder()
                    .WithDetails(src.Code, src.Description, string.Empty)
                    .WithAmount(context.Mapper.Map<Amount>(src.Amount))
                    .WithCouponDetailId(context.Mapper.Map<CouponDetailId>(src.CouponDetailId))
                    .Build();
            });

        CreateMap<CouponExtendedCreateDto, CouponAggreagete>()
            .ConstructUsing((src, context) =>
            {
                return new CouponBuilder()
                    .WithDetails(src.Code, src.Description, src.Detail)
                    .WithAmount(context.Mapper.Map<Amount>(src.Amount))
                    .WithCouponDetailId(context.Mapper.Map<CouponDetailId>(src.CouponDetailId))
                    .Build();
            });

        CreateMap<CouponAggreagete, CouponSimpleDto>();

        CreateMap<CouponAggreagete, CouponWithAllRelationsDto>();

        CreateMap<CouponId, CouponIdDto>().ReverseMap();

        CreateMap<Amount, AmountDto>().ReverseMap();
    }

    #endregion
}