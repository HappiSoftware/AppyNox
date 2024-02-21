using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Domain.Coupons;
using AutoMapper;
using CouponAggreagete = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Profiles;

public class CouponProfile : Profile
{
    #region [ Public Constructors ]

    public CouponProfile()
    {
        CreateMap<CouponSimpleCreateDto, CouponAggreagete>()
            .ConstructUsing((src, context) => CouponAggreagete.Create(
                src.Code,
                src.Description,
                null,
                context.Mapper.Map<Amount>(src.Amount),
                context.Mapper.Map<CouponDetailId>(src.CouponDetailEntityId)
            ));

        CreateMap<CouponExtendedCreateDto, CouponAggreagete>()
            .ConstructUsing((src, context) => CouponAggreagete.Create(
                src.Code,
                src.Description,
                src.Detail,
                context.Mapper.Map<Amount>(src.Amount),
                context.Mapper.Map<CouponDetailId>(src.CouponDetailEntityId)
            ));

        CreateMap<CouponAggreagete, CouponSimpleDto>();

        CreateMap<CouponAggreagete, CouponWithAllRelationsDto>();

        CreateMap<CouponId, CouponIdDto>().ReverseMap();

        CreateMap<Amount, AmountDto>().ReverseMap();

        CreateMap<NoxAuditData, NoxAuditDataDto>();
    }

    #endregion
}