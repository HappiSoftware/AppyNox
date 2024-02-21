using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;

[CouponDetailLevel(CouponUpdateDetailLevel.Simple)]
public class CouponSimpleUpdateDto : CouponSimpleCreateDto
{
    #region [ Properties ]

    public CouponIdDto Id { get; set; } = default!;

    #endregion
}