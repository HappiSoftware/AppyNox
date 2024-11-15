using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.ValueObjects;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models;

public class CouponDetailTagDto
{
    #region [ Properties ]

    public CouponDetailTagIdDto Id { get; set; } = default!;

    public string Tag { get; set; } = string.Empty;

    #endregion
}