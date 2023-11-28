using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base
{
    [CouponDetailLevel(CouponCreateDetailLevel.Simple)]
    public class CouponSimpleCreateDto : BaseDto
    {
        #region [ Properties ]

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public Guid CouponDetailEntityId { get; set; }

        #endregion
    }
}