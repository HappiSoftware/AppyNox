﻿using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.Exceptions;
using AppyNox.Services.Base.Infrastructure.Exceptions.Interfaces;
using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Sso.Application.DTOs.RefreshTokenDtos.Models;
using AppyNox.Services.Sso.Application.Interfaces.Authentication;
using AppyNox.Services.Sso.Application.MediatR.Commands;
using AppyNox.Services.Sso.Infrastructure.Exceptions;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;
using AppyNox.Services.Sso.WebAPI.Localization;
using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AppyNox.Services.Sso.WebAPI.Controllers.Authentication;

[ApiController]
[ApiVersion(NoxVersions.v1_0)]
[Route("api/v{version:apiVersion}/[controller]")]
[AllowAnonymous]
public class AuthenticationController(
    ICustomUserManager customUserManager,
    ICustomTokenManager customTokenManager,
    IValidator<LoginDto> loginDtoValidator,
    IValidator<RefreshTokenDto> refreshTokenDtoValidator,
    IMediator mediator) : Controller
{
    #region [ Fields ]

    private readonly ICustomUserManager _customUserManager = customUserManager;

    private readonly ICustomTokenManager _customTokenManager = customTokenManager;

    private readonly IValidator<LoginDto> _loginDtoValidator = loginDtoValidator;

    private readonly IValidator<RefreshTokenDto> _refreshTokenDtoValidator = refreshTokenDtoValidator;
    
    private readonly IMediator _mediator = mediator;

    #endregion

    #region [ JWT Operations ]

    [HttpPost]
    [Route("connect/token")]
    public async Task<NoxApiResponse> Authenticate([FromBody] LoginDto userCredential)
    {
        var result = await _mediator.Send(new AuthenticateCommand(userCredential));

        return new NoxApiResponse(new { result.Token, result.RefreshToken }, NoxSsoApiResourceService.SignInSuccessful);
    }

    [HttpGet]
    [Route("verify-token/{token}")]
    public async Task<IActionResult> Verify(string token, string audience)
    {
        await _customTokenManager.VerifyToken(token, audience);
        return NoContent();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto model)
    {
        var validationResult = await _refreshTokenDtoValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            throw new NoxFluentValidationException(typeof(RefreshTokenDto), validationResult);
        }

        try
        {
            await _customTokenManager.VerifyToken(model.Token, model.Audience);
        }
        catch (Exception ex) when (ex is INoxTokenExpiredException)
        {
            // expected, do not take action
        }
        catch (Exception ex) when (ex is INoxAuthenticationException)
        {
            throw;
        }

        var userId = _customTokenManager.GetUserInfoByToken(model.Token, model.Audience);
        var storedRefreshToken = await _customUserManager.RetrieveStoredRefreshToken(userId);

        if (!_customTokenManager.VerifyRefreshToken(model.RefreshToken, storedRefreshToken))
        {
            throw new NoxSsoApiException(NoxSsoApiResourceService.RefreshTokenInvalid, (int)NoxSsoApiExceptionCode.RefreshTokenInvalid, (int)HttpStatusCode.Unauthorized);
        }

        var newJwtToken = await _customTokenManager.CreateToken(userId, model.Audience);
        var newRefreshToken = _customTokenManager.CreateRefreshToken();
        await _customUserManager.SaveRefreshToken(userId, newRefreshToken);

        return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
    }

    #endregion
}