using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using AppyNox.Services.Authentication.Infrastructure.Data;

namespace AppyNox.Services.Authentication.Application.Validators.SharedRules
{
    public static class GenericValidationRules
    {
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
    }
}
