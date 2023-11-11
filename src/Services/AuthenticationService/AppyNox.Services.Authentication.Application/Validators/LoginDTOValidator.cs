using AppyNox.Services.Authentication.Application.Account;
using FluentValidation;

namespace AppyNox.Services.Authentication.Application.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        #region [ Public Constructors ]

        public LoginDTOValidator()
        {
            RuleFor(user => user.UserName).NotNull().NotEmpty().NotEmpty().WithMessage("Username is required.").WithErrorCode("422");
            RuleFor(login => login.Password).NotNull().NotEmpty().NotEmpty().WithMessage("Password is required.").WithErrorCode("422");
        }

        #endregion
    }
}