using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Profiles;

public class CouponDetailProfile : Profile
{
    #region [ Public Constructors ]

    public CouponDetailProfile()
    {
        CreateMap<CouponDetailCompositeCreateDto, CouponDetail>()
            .ConstructUsing((src, context) =>
            {
                return new CouponDetailBuilder()
                    .WithDetails(src.Code, src.Detail)
                    .WithTags(context.Mapper.Map<ICollection<CouponDetailTag>>(src.CouponDetailTags))
                    .Build();
            });

        CreateMap<CouponDetail, CouponDetailDto>().ReverseMap();
        CreateMap<CouponDetailId, CouponDetailIdDto>().ReverseMap();
    }

    #endregion
}