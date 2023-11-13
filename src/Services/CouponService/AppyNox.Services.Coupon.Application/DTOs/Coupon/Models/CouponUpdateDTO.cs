namespace AppyNox.Services.Coupon.Application.Dtos.Coupon.Models
{
    public class CouponUpdateDto : CouponCreateDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}