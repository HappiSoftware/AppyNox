using AppyNox.Services.Coupon.Application.Dtos.Coupon.Models;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.Coupon.Mappings
{
    public class CouponMapping : Profile
    {
        #region [ Public Constructors ]

        public CouponMapping()
        {
            CreateMap<CouponCreateDto, CouponEntity>();

            CreateMap<CouponUpdateDto, CouponEntity>();

            CreateMap<CouponEntity, CouponDto>();

            CreateMap<CouponEntity, CouponWithIdDto>().ReverseMap();
        }

        #endregion
    }
}