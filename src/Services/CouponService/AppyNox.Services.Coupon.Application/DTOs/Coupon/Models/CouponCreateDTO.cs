namespace AppyNox.Services.Coupon.Application.Dtos.Coupon.Models
{
    public class CouponCreateDto : BaseDto
    {
        #region [ Properties ]

        public string Code { get; set; } = string.Empty;

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        #endregion
    }
}