using AppyNox.Services.Coupon.Application.DTOs.Coupon.Models;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Mappings
{
    public class CouponMapping : Profile
    {
        #region [ Public Constructors ]

        public CouponMapping()
        {
            CreateMap<CouponCreateDTO, CouponEntity>();

            CreateMap<CouponUpdateDTO, CouponEntity>();

            CreateMap<CouponEntity, CouponDTO>();

            CreateMap<CouponEntity, CouponWithIdDTO>().ReverseMap();
        }

        #endregion
    }
}