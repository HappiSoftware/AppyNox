using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;

[CouponDetailLevel(CouponUpdateDetailLevel.Extended)]
public class CouponExtendedUpdateDto : CouponExtendedCreateDto
{
    #region Properties

    public CouponIdDto Id { get; set; } = default!;

    #endregion
}