using AppyNox.Services.Coupon.Application.Dtos.Coupon.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.Coupon.Models
{
    [CouponDetailLevel(CouponDetailLevel.Basic)]
    public class CouponDto : BaseDto
    {
        #region [ Properties ]

        public string Code { get; set; } = string.Empty;

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        #endregion
    }
}