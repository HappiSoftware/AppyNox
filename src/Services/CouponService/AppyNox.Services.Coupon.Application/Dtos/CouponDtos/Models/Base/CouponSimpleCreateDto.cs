using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base
{
    [CouponDetailLevel(CouponCreateDetailLevel.Simple)]
    public class CouponSimpleCreateDto : IHasCode
    {
        #region [ Properties ]

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public string Description { get; set; } = string.Empty;

        public Guid CouponDetailEntityId { get; set; }

        #endregion

        #region [ IHasCode ]

        public string Code { get; set; } = string.Empty;

        #endregion
    }
}