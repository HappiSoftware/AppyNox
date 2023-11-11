namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Models
{
    public class CouponUpdateDTO : CouponCreateDTO, IUpdateDTO
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}