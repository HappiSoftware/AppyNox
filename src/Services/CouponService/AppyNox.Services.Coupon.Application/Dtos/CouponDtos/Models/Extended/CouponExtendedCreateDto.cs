using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;

[CouponDetailLevel(CouponCreateDetailLevel.Extended)]
public class CouponExtendedCreateDto : CouponSimpleCreateDto
{
    #region [ Properties ]

    public string? Detail { get; set; }

    #endregion
}