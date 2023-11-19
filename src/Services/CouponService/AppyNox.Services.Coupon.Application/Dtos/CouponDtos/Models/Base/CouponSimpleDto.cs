using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base
{
    [CouponDetailLevel(CouponDataAccessDetailLevel.Simple)]
    public class CouponSimpleDto : BaseDto
    {
        #region [ Properties ]

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public string? Description { get; set; }

        #endregion
    }
}