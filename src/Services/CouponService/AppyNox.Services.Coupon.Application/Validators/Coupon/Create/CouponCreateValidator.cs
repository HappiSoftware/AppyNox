using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators.Coupon.Create
{
    public class CouponCreateValidator : DtoValidatorBase<CouponCreateDto>
    {
        #region [ Public Constructors ]

        public CouponCreateValidator()
        {
            RuleFor(coupon => coupon.Amount.DiscountAmount)
                .NotNull().WithMessage("Discount Amount can not be null")
                .GreaterThan(0).WithMessage("Discount Amount can not be equal or less then 0.");

            RuleFor(coupon => coupon.Amount.MinAmount)
                .NotNull().WithMessage("MinAmount cannot be null")
                .GreaterThan(0).WithMessage("MinAmount can not be equal equal or less then 0.");

            RuleFor(coupon => coupon.Description)
                .NotNull().WithMessage("Description can not be null");

            RuleFor(coupon => coupon.Detail)
                .MaximumLength(60).When(coupon => !coupon.Detail.IsNullOrEmpty()).WithMessage("Detail cannot be longer than 60 characters");
        }

        #endregion
    }
}