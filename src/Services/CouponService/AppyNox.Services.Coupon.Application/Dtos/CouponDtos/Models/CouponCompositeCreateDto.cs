using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.ValueObjects;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;

public class CouponCompositeCreateDto : IHasCode
{
    #region [ Properties ]

    public string Description { get; set; } = string.Empty;

    public AmountDto Amount { get; set; } = default!;

    public CouponDetailCompositeCreateDto CouponDetail { get; set; } = default!;

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}