using AppyNox.Services.Coupon.Application.DTOs.Coupon.DetailLevel;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Models
{
    [CouponDetailLevel(CouponDetailLevel.Basic)]
    public class CouponDTO : BaseDTO
    {
        #region [ Properties ]

        public string Code { get; set; } = string.Empty;

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        #endregion
    }
}