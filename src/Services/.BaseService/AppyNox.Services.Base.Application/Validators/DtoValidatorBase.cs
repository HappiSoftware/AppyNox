using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Validators.SharedRules;
using AppyNox.Services.Base.Domain.Interfaces;
using FluentValidation;

namespace AppyNox.Services.Base.Application.Validators
{
    /// <summary>
    /// Provides a Fluent Validation validator for Dto
    /// </summary>
    /// <typeparam name="T">The type of the DTO being validated</typeparam>
    public class DtoValidatorBase<T> : AbstractValidator<T> where T : class
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoValidatorBase{T}"/> class,
        /// setting up validation rules for DtoBase properties.
        /// </summary>
        public DtoValidatorBase()
        {
            if (typeof(IHasCode).IsAssignableFrom(typeof(T)))
            {
                RuleFor(dto => dto).NotNull().WithMessage("Dto cannot be null.");
                RuleFor(dto => (dto as IHasCode)!.Code).ValidateCode();
            }

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