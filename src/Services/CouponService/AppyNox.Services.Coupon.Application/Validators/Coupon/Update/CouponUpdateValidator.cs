using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using AppyNox.Services.Coupon.Application.Validators.Coupon.Create;

namespace AppyNox.Services.Coupon.Application.Validators.Coupon.Update
{
    public class CouponUpdateValidator : DtoValidatorBase<CouponUpdateDto>
    {
        #region [ Public Constructors ]

        public CouponUpdateValidator(CouponCreateValidator validator)
        {
            RuleFor(o => o)
                .SetValidator(validator);
        }

        #endregion
    }
}