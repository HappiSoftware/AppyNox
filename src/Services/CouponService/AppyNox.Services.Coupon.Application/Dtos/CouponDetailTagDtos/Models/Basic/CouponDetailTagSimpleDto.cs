using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic;

[CouponDetailTagDetailLevel(CouponDetailTagDataAccessDetailLevel.Simple)]
public class CouponDetailTagSimpleDto
{
    #region [ Properties ]

    public CouponDetailTagIdDto Id { get; set; } = default!;

    public string Tag { get; set; } = string.Empty;

    #endregion
}