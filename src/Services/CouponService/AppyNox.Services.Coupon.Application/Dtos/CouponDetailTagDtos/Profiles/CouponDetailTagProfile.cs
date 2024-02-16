using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Profiles;

public class CouponDetailTagProfile : Profile
{
    #region [ Public Constructors ]

    public CouponDetailTagProfile()
    {
        CreateMap<CouponDetailTag, CouponDetailTagSimpleDto>().ReverseMap();
    }

    #endregion
}