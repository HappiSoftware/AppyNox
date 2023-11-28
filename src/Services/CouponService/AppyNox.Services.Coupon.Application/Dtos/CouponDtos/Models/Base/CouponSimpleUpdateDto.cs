using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base
{
    [CouponDetailLevel(CouponUpdateDetailLevel.Simple)]
    public class CouponSimpleUpdateDto : CouponSimpleCreateDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}