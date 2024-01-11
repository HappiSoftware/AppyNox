using AppyNox.Services.Authentication.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.Validators.SharedRules
{
    /// <summary>
    /// Provides generic validation rules for various DTOs using FluentValidation.
    /// </summary>
    public static class GenericValidationRules
    {
        #region [ Public Methods ]

        /// <summary>
        /// Validates the email format and checks for its uniqueness in the database.
        /// </summary>
        /// <param name="ruleBuilder">The rule builder for the validation rule.</param>
        /// <param name="dbContext">The database context to check for email uniqueness.</param>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <returns>An IRuleBuilderOptions instance for further rule configuration.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the dbContext is null.</exception>
        public static IRuleBuilderOptions<T, string> CheckEmailValidity<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            IDatabaseChecks userUniquenessChecker)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(userUniquenessChecker.IsEmailUniqueAsync)
                .WithMessage("Email already exists.");
        }

        /// <summary>
        /// Validates the username and checks for its uniqueness in the database.
        /// </summary>
        /// <param name="ruleBuilder">The rule builder for the validation rule.</param>
        /// <param name="dbContext">The database context to check for username uniqueness.</param>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <returns>An IRuleBuilderOptions instance for further rule configuration.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the dbContext is null.</exception>
        public static IRuleBuilderOptions<T, string> CheckUsernameValidity<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            IDatabaseChecks validatorDatabaseChecker)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Username is required.")
                .MustAsync(validatorDatabaseChecker.IsUsernameUniqueAsync)
                .WithMessage("Username already exists.");
        }

        public static IRuleBuilderOptions<T, string> BeAValidPassword<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            IPasswordValidator<ApplicationUser> passwordValidator,
            UserManager<ApplicationUser> userManager)
        {
            return ruleBuilder.MustAsync(async (dto, password, cancellationToken) =>
            {
                var result = await passwordValidator.ValidateAsync(userManager, new ApplicationUser(), password);
                return result.Succeeded;
            }).WithMessage("Password does not meet the required criteria.");
        }

        #endregion
    }
}