using FluentValidation;

namespace AppyNox.Services.Coupon.Application.Validators.SharedRules
{
    public static class GenericValidationRules
    {
        #region [ Public Methods ]

        public static IRuleBuilderOptions<T, string?> ValidateDescription<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().NotEmpty().WithMessage("Description cannot be null")
                .MaximumLength(60).WithMessage("Description cannot be longer than 60 characters");
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