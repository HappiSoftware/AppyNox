using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic;

// No need for detail level for nested bulk dtos
public class CouponDetailTagBulkCreateDto
{
    #region [ Properties ]

    public string Tag { get; set; } = string.Empty;

    #endregion
}