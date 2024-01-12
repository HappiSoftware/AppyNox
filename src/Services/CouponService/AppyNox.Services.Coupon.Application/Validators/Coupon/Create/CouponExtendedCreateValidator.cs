using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators.Coupon.Create
{
    public class CouponExtendedCreateValidator : BaseDtoValidator<CouponExtendedCreateDto>
    {
        #region [ Public Constructors ]

        public CouponExtendedCreateValidator(CouponSimpleCreateValidator validator)
        {
            RuleFor(o => o)
                .SetValidator(validator);

            RuleFor(coupon => coupon.Detail)
                .MaximumLength(60).When(coupon => !coupon.Detail.IsNullOrEmpty()).WithMessage("Detail cannot be longer than 60 characters");
        }

        #endregion
    }
}