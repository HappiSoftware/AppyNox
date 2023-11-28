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
            CreateMap<CouponSimpleCreateDto, CouponEntity>();

            CreateMap<CouponExtendedCreateDto, CouponEntity>();

            CreateMap<CouponSimpleUpdateDto, CouponEntity>().ReverseMap();

            CreateMap<CouponEntity, CouponSimpleDto>();

            CreateMap<CouponEntity, CouponWithAllRelationsDto>();
        }

        #endregion
    }
}