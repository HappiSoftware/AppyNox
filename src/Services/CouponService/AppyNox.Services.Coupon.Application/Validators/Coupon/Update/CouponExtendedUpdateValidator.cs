using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Application.Validators.Coupon.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Validators.Coupon.Update
{
    public class CouponExtendedUpdateValidator : BaseDtoValidator<CouponExtendedUpdateDto>
    {
        #region Public Constructors

        public CouponExtendedUpdateValidator(CouponExtendedCreateValidator validator)
        {
            RuleFor(o => o)
                .SetValidator(validator);
        }

        #endregion
    }
}