using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Profiles;

public class CouponDetailTagProfile : Profile
{
    #region [ Public Constructors ]

    public CouponDetailTagProfile()
    {
        CreateMap<CouponDetailTagBulkCreateDto, CouponDetailTag>()
            .ConstructUsing((src, context) =>
            {
                return new CouponDetailTagBuilder()
                    .WithDetails(src.Tag)
                    .MarkAsBulkCreate()
                    .Build();
            });

        CreateMap<CouponDetailTag, CouponDetailTagSimpleDto>().ReverseMap();
        CreateMap<CouponDetailTagId, CouponDetailTagIdDto>().ReverseMap();
    }

    #endregion
}