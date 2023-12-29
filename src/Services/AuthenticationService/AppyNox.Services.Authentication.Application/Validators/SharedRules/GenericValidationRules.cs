using AppyNox.Services.Authentication.Infrastructure.Data;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

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
        public static IRuleBuilderOptions<T, string> CheckEmailValidity<T>(this IRuleBuilder<T, string> ruleBuilder, IdentityDbContext? dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext), "Database context was null.");
            }

            return (IRuleBuilderOptions<T, string>)ruleBuilder
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .CustomAsync(async (email, context, cancellationToken) =>
            {
                if (await dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken))
                {
                    var failure = new ValidationFailure("Email", "Email already exists.");
                    failure.ErrorCode = "EMAIL_ALREADY_EXISTS"; // Custom error code
                    context.AddFailure(failure);
                }
            });
        }

        /// <summary>
        /// Validates the username and checks for its uniqueness in the database.
        /// </summary>
        /// <param name="ruleBuilder">The rule builder for the validation rule.</param>
        /// <param name="dbContext">The database context to check for username uniqueness.</param>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <returns>An IRuleBuilderOptions instance for further rule configuration.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the dbContext is null.</exception>
        public static IRuleBuilderOptions<T, string> CheckUserNameValidity<T>(this IRuleBuilder<T, string> ruleBuilder, IdentityDbContext? dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext), "Database context was null.");
            }

            return (IRuleBuilderOptions<T, string>)ruleBuilder
            .NotEmpty().WithMessage("Username is required.")
            .CustomAsync(async (username, context, cancellationToken) =>
            {
                if (await dbContext.Users.AnyAsync(x => x.UserName == username, cancellationToken))
                {
                    var failure = new ValidationFailure("UserName", "Username already exists.");
                    failure.ErrorCode = "USERNAME_ALREADY_EXISTS"; // Custom error code
                    context.AddFailure(failure);
                }
            });
        }

        #endregion
    }
}