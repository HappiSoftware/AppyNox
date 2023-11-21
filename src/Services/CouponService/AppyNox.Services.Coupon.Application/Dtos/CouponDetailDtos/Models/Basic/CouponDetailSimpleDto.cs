using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel;


namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic
{
    [CouponDetailDetailLevel(CouponDetailDataAccessDetailLevel.Simple)]
    public class CouponDetailSimpleDto : BaseDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}