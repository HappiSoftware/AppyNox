using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base
{
    public class CouponBasicCreateDto : BaseDto
    {
        #region [ Properties ]

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public string? Description { get; set; }

        public Guid CouponDetailEntityId { get; set; }

        #endregion
    }
}