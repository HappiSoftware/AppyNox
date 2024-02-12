using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic
{
    [CouponDetailDetailLevel(CouponDetailDataAccessDetailLevel.Simple)]
    public class CouponDetailSimpleDto : IHasCode
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        // Omitted for simplicity

        #endregion

        #region [ IHasCode ]

        public string Code { get; set; } = string.Empty;

        #endregion
    }
}