using AppyNox.Services.Coupon.Application.DTOs;
using AppyNox.Services.Coupon.Application.Validators.SharedRules;
using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators
{
    public class BaseDTOValidator<T> : AbstractValidator<T> where T : BaseDTO
    {
        #region [ Public Constructors ]

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

        #endregion
    }
}