namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Models
{
    public class CouponCreateDTO : BaseDTO
    {
        #region [ Properties ]

        public string Code { get; set; } = string.Empty;

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        #endregion
    }
}