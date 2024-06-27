using AppyNox.Services.Sso.Application.AsyncLocals;
using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Sso.Application.Interfaces.Authentication;
using AppyNox.Services.Sso.Application.MediatR.Commands;
using FluentValidation;
using MediatR;

namespace AppyNox.Services.Sso.Application.MediatR.Handlers;

public sealed class AuthenticateCommandHandler(
    ICustomUserManager customUserManager,
    IValidator<LoginDto> loginDtoValidator) : IRequestHandler<AuthenticateCommand, AuthenticateResult>
{
    private readonly ICustomUserManager _customUserManager = customUserManager;
    private readonly IValidator<LoginDto> _loginDtoValidator = loginDtoValidator;

    public async Task<AuthenticateResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        SsoContext.IsConnectRequest = true;

        var validationResult = await _loginDtoValidator.ValidateAsync(request.UserCredential, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var (jwtToken, refreshToken) = await _customUserManager.Authenticate(request.UserCredential);

        if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
        {
            throw new ApplicationException(ResourceService.AuthenticationFailed);
        }

        return new AuthenticateResult(jwtToken, refreshToken);
    }
}