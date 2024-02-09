using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using FluentValidation;

namespace AppyNox.Services.Sso.Application.Validators.Account
{
    /// <summary>
    /// Validator class for LoginDto using FluentValidation.
    /// </summary>
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        #region [ Public Constructors ]

        public LoginDtoValidator()
        {
            RuleFor(user => user.UserName).NotNull().NotEmpty().NotEmpty().WithMessage("Username is required.").WithErrorCode("422");
            RuleFor(login => login.Password).NotNull().NotEmpty().NotEmpty().WithMessage("Password is required.").WithErrorCode("422");
            RuleFor(login => login.Audience).NotNull().NotEmpty().NotEmpty().WithMessage("Audience is required.").WithErrorCode("422");
        }

        #endregion
    }
}