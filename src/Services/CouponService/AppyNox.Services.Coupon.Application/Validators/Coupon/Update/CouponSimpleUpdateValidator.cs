using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Validators.Coupon.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Validators.Coupon.Update
{
    public class CouponSimpleUpdateValidator : DtoValidatorBase<CouponSimpleUpdateDto>
    {
        #region Public Constructors

        public CouponSimpleUpdateValidator(CouponSimpleCreateValidator validator)
        {
            RuleFor(o => o)
                .SetValidator(validator);
        }

        #endregion
    }
}