using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models.Basic
{
    [CouponDetailTagDetailLevel(CouponDetailTagDataAccessDetailLevel.Simple)]
    public class CouponDetailTagSimpleDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        //public string Tag { get; set; } = string.Empty;

        #endregion
    }
}