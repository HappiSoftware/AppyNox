using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;

[CouponDetailDetailLevel(CouponDetailDataAccessDetailLevel.Simple)]
public class CouponDetailSimpleDto : IHasCode
{
    #region [ Properties ]

    public CouponDetailIdDto Id { get; set; } = default!;

    // Omitted for simplicity

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public IEnumerable<CouponDetailTagSimpleDto>? CouponDetailTags { get; set; }

    #endregion
}