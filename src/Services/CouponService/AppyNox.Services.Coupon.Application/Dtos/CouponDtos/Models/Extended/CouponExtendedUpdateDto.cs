using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Domain.Entities;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;

[CouponDetailLevel(CouponUpdateDetailLevel.Extended)]
public class CouponExtendedUpdateDto : CouponExtendedCreateDto
{
    #region Properties

    public CouponId Id { get; set; } = default!;

    #endregion
}