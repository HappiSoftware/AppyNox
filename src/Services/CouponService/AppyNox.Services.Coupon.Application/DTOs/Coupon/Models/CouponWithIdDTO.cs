using AppyNox.Services.Coupon.Application.DTOs.Coupon.DetailLevel;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Models
{
    [CouponDetailLevel(CouponDetailLevel.WithId)]
    public class CouponWithIdDTO : CouponDTO, IUpdateDTO
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public int DetailValue { get; set; }

        #endregion
    }
}