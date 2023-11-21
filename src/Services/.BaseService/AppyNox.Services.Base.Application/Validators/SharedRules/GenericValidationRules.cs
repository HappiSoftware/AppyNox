using FluentValidation;

namespace AppyNox.Services.Base.Application.Validators.SharedRules
{
    public static class GenericValidationRules
    {
        #region [ Public Methods ]

        public static IRuleBuilderOptions<T, string?> ValidateCode<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().NotEmpty().WithMessage("Code cannot be null")
                .Length(5).WithMessage("Code should be 5 characters");
        }

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