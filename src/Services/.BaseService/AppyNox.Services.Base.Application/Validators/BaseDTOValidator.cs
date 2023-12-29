using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Validators.SharedRules;
using FluentValidation;

namespace AppyNox.Services.Base.Application.Validators
{
    /// <summary>
    /// Provides a Fluent Validation validator for BaseDto and its derived types.
    /// </summary>
    /// <typeparam name="T">The type of the DTO being validated, which must be derived from BaseDto.</typeparam>
    public class BaseDtoValidator<T> : AbstractValidator<T> where T : BaseDto
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDtoValidator{T}"/> class,
        /// setting up validation rules for BaseDto properties.
        /// </summary>
        public BaseDtoValidator()
        {
            RuleFor(dto => dto.Code).ValidateCode();

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