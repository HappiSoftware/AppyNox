using AppyNox.Services.Sso.Application.DTOs.RefreshTokenDtos.Models;
using FluentValidation;

namespace AppyNox.Services.Sso.Application.Validators.Account;

/// <summary>
/// Validator class for RefreshTokenDto using FluentValidation.
/// </summary>
public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    #region [ Public Constructors ]

    public RefreshTokenDtoValidator()
    {
        RuleFor(user => user.Token).NotNull().NotEmpty().WithMessage("Token is required.");
        RuleFor(login => login.RefreshToken).NotNull().NotEmpty().WithMessage("Refresh Token is required.");
        RuleFor(login => login.Audience).NotNull().NotEmpty().WithMessage("Audience is required.");
    }

    #endregion
}