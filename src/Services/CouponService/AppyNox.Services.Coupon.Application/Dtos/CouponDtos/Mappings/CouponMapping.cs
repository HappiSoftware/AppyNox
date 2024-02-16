using AppyNox.Services.Base.Application.Extensions;
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
            CreateMap<CouponSimpleCreateDto, CouponEntity>().ReverseMap();

            CreateMap<CouponExtendedCreateDto, CouponEntity>();

            CreateMap<CouponSimpleUpdateDto, CouponEntity>().ReverseMap();

            CreateMap<CouponExtendedUpdateDto, CouponEntity>().ReverseMap();

            CreateMap<CouponEntity, CouponSimpleDto>().MapAuditInfo().ReverseMap();

            CreateMap<CouponEntity, CouponWithAllRelationsDto>();

            CreateMap<CouponId, CouponIdDto>().ReverseMap();
        }

        #endregion
    }
}