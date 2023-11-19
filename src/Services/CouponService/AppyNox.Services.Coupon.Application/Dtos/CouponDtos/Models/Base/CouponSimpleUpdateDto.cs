namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base
{
    public class CouponSimpleUpdateDto : CouponBasicCreateDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}