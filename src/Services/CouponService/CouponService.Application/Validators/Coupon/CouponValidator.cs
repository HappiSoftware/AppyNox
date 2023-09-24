using CouponService.Application.DTOs.Coupon.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Application.Validators.Coupon
{
    public class CouponValidator : BaseDTOValidator<CouponCreateDTO>
    {
        public CouponValidator()
        {
            RuleFor(coupon => coupon.Code)
                .NotNull().NotEmpty().WithMessage("Code cannot be null")
                .MaximumLength(5).WithMessage("Code cannot be longer than 5 characters");

            RuleFor(coupon => coupon.DiscountAmount)
                .NotNull().WithMessage("Discount Amount cannot be null")
                .NotEqual(0).WithMessage("Discount Amount cannot be equal to 0."); ;

            RuleFor(coupon => coupon.MinAmount)
                .NotNull().WithMessage("MinAmount cannot be null")
                .NotEqual(0).WithMessage("MinAmount cannot be equal to 0.");
        }
    }
}
