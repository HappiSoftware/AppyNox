using AppyNox.Services.Coupon.Application.Dtos.Coupon.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.Coupon.Models
{
    [CouponDetailLevel(CouponDetailLevel.WithId)]
    public class CouponWithIdDto : CouponDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public int DetailValue { get; set; }

        #endregion
    }
}