using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.ValueObjects;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;

public class CouponCreateDto : IHasCode
{
    #region [ Properties ]

    public string Description { get; set; } = string.Empty;

    public AmountDto Amount { get; set; } = default!;

    public CouponDetailIdDto CouponDetailId { get; set; } = default!;

    public string? Detail { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}