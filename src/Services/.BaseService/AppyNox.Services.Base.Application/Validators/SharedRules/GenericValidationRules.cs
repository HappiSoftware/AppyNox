using FluentValidation;

namespace AppyNox.Services.Base.Application.Validators.SharedRules
{
    /// <summary>
    /// Provides common validation rules for various fields used across different DTOs.
    /// </summary>
    public static class GenericValidationRules
    {
        #region [ Public Methods ]

        /// <summary>
        /// Defines validation rules for a 'Code' field.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder for the 'Code' field.</param>
        /// <returns>The configured rule builder options.</returns>
        public static IRuleBuilderOptions<T, string?> ValidateCode<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().NotEmpty().WithMessage("Code cannot be null")
                .Length(5).WithMessage("Code should be 5 characters");
        }

        /// <summary>
        /// Defines validation rules for an 'Id' field.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder for the 'Id' field.</param>
        /// <returns>The configured rule builder options.</returns>
        public static IRuleBuilderOptions<T, Guid> ValidateId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder
                .NotNull()
                .NotEmpty()
                .WithMessage("Id cannot be null");
        }

        #endregion
    }
}