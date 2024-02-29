using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;

[CouponDetailLevel(CouponDataAccessDetailLevel.Simple)]
public class CouponSimpleDto : CouponSimpleCreateDto, INoxAuditDto
{
    #region [ IAuditDto ]

    public NoxAuditDataDto Audit { get; set; } = default!;

    #endregion
}