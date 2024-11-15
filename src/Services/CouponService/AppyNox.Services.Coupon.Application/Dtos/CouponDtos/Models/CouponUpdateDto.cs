using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.ValueObjects;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;

public class CouponUpdateDto : CouponCreateDto
{
    #region [ Properties ]

    public CouponIdDto Id { get; set; } = default!;

    #endregion
}