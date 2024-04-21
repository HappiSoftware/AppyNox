using AppyNox.Services.Sso.Application.DTOs.ApplicationUserDTOs.Models;
using AppyNox.Services.Sso.Application.Validators.SharedRules;
using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Base.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Sso.Application.Validators.ApplicationUserValidators
{
    /// <summary>
    /// Validator class for IdentityUserCreateDto using FluentValidation.
    /// Includes rules for validating user creation data.
    /// </summary>
    public class ApplicationUserCreateDtoValidator : DtoValidatorBase<ApplicationUserCreateDto>
    {
        #region [ Public Constructors ]

        public ApplicationUserCreateDtoValidator(IDatabaseChecks databaseChecks,
                                              IPasswordValidator<ApplicationUser> passwordValidator,
                                              UserManager<ApplicationUser> userManager)
        {
            RuleFor(user => user.UserName).CheckUsernameValidity(databaseChecks);
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password required").WithErrorCode("PASSWORD_REQUIRED");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Passwords should match").WithErrorCode("PASSWORDS_SHOULD_MATCH");
            RuleFor(x => x.Password).BeAValidPassword(passwordValidator, userManager);
            RuleFor(x => x.Email).CheckEmailValidity(databaseChecks);
            RuleFor(x => x.Name)
                .NotNull().NotEmpty().WithMessage("Name required")
                .MaximumLength(30).WithMessage("Name must not exceed 30 characters");
            RuleFor(x => x.Surname)
                .NotNull().NotEmpty().WithMessage("Surname required")
                .MaximumLength(15).WithMessage("Surname must not exceed 15 characters");
        }

        #endregion
    }
}