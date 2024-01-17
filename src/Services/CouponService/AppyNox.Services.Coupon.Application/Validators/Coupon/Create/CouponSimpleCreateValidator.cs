using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators.Coupon.Create
{
    public class CouponSimpleCreateValidator : DtoValidatorBase<CouponSimpleCreateDto>
    {
        #region [ Public Constructors ]

        public CouponSimpleCreateValidator()
        {
            RuleFor(coupon => coupon.DiscountAmount)
                .NotNull().WithMessage("Discount Amount cannot be null")
                .NotEqual(0).WithMessage("Discount Amount cannot be equal to 0.");

            RuleFor(coupon => coupon.MinAmount)
                .NotNull().WithMessage("MinAmount cannot be null")
                .NotEqual(0).WithMessage("MinAmount cannot be equal to 0.");
        }

        #endregion
    }
}