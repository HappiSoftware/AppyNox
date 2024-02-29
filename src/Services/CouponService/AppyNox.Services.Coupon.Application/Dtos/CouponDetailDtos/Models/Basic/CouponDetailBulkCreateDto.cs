using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;

// No need for detail level for nested bulk dtos
public class CouponDetailBulkCreateDto
{
    #region [ Properties ]

    public string? Detail { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public IEnumerable<CouponDetailTagBulkCreateDto>? CouponDetailTags { get; set; }

    #endregion
}