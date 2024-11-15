using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models;

// No need for detail level for nested bulk dtos
public class CouponDetailCompositeCreateDto
{
    #region [ Properties ]

    public string? Detail { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public IEnumerable<CouponDetailTagCompositeCreateDto>? CouponDetailTags { get; set; }

    #endregion
}