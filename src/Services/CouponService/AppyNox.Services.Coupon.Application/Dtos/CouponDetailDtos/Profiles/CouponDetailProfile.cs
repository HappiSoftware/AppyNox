using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Coupons;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Profiles;

public class CouponDetailProfile : Profile
{
    #region [ Public Constructors ]

    public CouponDetailProfile()
    {
        CreateMap<CouponDetail, CouponDetailSimpleDto>().ReverseMap();
        CreateMap<CouponDetailId, CouponDetailIdDto>().ReverseMap();
    }

    #endregion
}