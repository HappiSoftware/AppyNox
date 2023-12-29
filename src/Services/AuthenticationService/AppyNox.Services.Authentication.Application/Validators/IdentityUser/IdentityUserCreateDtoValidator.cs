using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Validators.SharedRules;
using AppyNox.Services.Authentication.Infrastructure.Data;
using FluentValidation;

namespace AppyNox.Services.Authentication.Application.Validators.IdentityUser
{
    /// <summary>
    /// Validator class for IdentityUserCreateDto using FluentValidation.
    /// Includes rules for validating user creation data.
    /// </summary>
    public class IdentityUserCreateDtoValidator : AbstractValidator<IdentityUserCreateDto>
    {
        #region [ Public Constructors ]

        public IdentityUserCreateDtoValidator(IdentityDbContext context)
        {
            RuleFor(user => user.UserName).CheckUserNameValidity(context);
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password required").WithErrorCode("PASSWORD_REQUIRED");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Passwords should match").WithErrorCode("PASSWORDS_SHOULD_MATCH");
            RuleFor(x => x.Email).CheckEmailValidity(context);
        }

        #endregion
    }
}