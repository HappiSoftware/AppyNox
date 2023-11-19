using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Mappings
{
    public class CouponMapping : Profile
    {
        #region [ Public Constructors ]

        public CouponMapping()
        {
            CreateMap<CouponBasicCreateDto, CouponEntity>();

            CreateMap<CouponSimpleUpdateDto, CouponEntity>();

            CreateMap<CouponEntity, CouponSimpleDto>();

            CreateMap<CouponEntity, CouponWithAllRelationsDto>();
        }

        #endregion
    }
}