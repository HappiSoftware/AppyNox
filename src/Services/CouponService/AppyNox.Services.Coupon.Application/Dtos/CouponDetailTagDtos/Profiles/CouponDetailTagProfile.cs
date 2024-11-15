using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Profiles;

public class CouponDetailTagProfile : Profile
{
    #region [ Public Constructors ]

    public CouponDetailTagProfile()
    {
        CreateMap<CouponDetailTagCompositeCreateDto, CouponDetailTag>()
            .ConstructUsing((src, context) =>
            {
                return new CouponDetailTagBuilder()
                    .WithDetails(src.Tag)
                    .MarkAsBulkCreate()
                    .Build();
            });

        CreateMap<CouponDetailTag, CouponDetailTagDto>().ReverseMap();
        CreateMap<CouponDetailTagId, CouponDetailTagIdDto>().ReverseMap();
    }

    #endregion
}