using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;

[CouponDetailLevel(CouponCreateDetailLevel.Simple)]
public class CouponSimpleCreateDto : IHasCode
{
    #region [ Properties ]

    public string Description { get; set; } = string.Empty;

    public AmountDto Amount { get; set; } = default!;

    public CouponDetailIdDto CouponDetailEntityId { get; set; } = default!;

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}