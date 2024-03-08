using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;

[CouponDetailLevel(CouponDataAccessDetailLevel.Simple)]
public class CouponSimpleDto : CouponSimpleCreateDto, IAuditDto
{
    #region [ IAuditDto ]

    public AuditInformation AuditInformation { get; set; } = default!;

    #endregion
}