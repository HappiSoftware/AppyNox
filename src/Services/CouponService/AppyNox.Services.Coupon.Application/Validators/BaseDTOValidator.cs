using AppyNox.Services.Coupon.Application.Dtos;
using AppyNox.Services.Coupon.Application.Validators.SharedRules;
using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators
{
    public class BaseDtoValidator<T> : AbstractValidator<T> where T : BaseDto
    {
        #region [ Public Constructors ]

        public BaseDtoValidator()
        {
            RuleFor(dto => dto.Description).ValidateDescription();

            if (typeof(IUpdateDto).IsAssignableFrom(typeof(T)))
            {
                // (IUpdateDto)updateDto might be null, first check that
                RuleFor(updateDto => updateDto).NotNull().WithMessage("Update Dto cannot be null.");

                // After check for id
                RuleFor(updateDto => ((IUpdateDto)updateDto).Id).ValidateId()
                    .When(updateDto => updateDto is IUpdateDto);
            }
        }

        #endregion
    }
}