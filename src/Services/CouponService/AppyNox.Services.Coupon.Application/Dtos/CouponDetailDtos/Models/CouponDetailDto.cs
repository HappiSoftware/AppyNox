using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models;

public class CouponDetailDto : IHasCode
{
    #region [ Properties ]

    public CouponDetailIdDto Id { get; set; } = default!;

    // Omitted for simplicity

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public ICollection<CouponDetailTagDto>? CouponDetailTags { get; set; }

    #endregion
}