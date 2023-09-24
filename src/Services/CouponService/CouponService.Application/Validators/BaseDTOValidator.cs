using CouponService.Application.DTOs;
using CouponService.Application.Validators.SharedRules;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Application.Validators
{
    public class BaseDTOValidator<T> : AbstractValidator<T> where T : BaseDTO
    {
        public BaseDTOValidator()
        {
            RuleFor(dto => dto.Description).ValidateDescription();

            if (typeof(IUpdateDTO).IsAssignableFrom(typeof(T)))
            {
                // (IUpdateDTO)updateDto might be null, first check that
                RuleFor(updateDto => updateDto).NotNull().WithMessage("Update DTO cannot be null.");

                // After check for id
                RuleFor(updateDto => ((IUpdateDTO)updateDto).Id).ValidateId()
                    .When(updateDto => updateDto is IUpdateDTO);

            }
        }
    }
}
