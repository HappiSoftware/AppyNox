using AppyNox.Services.Authentication.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Authentication.Application.DTOs.RefreshTokenDtos.Models;
using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AppyNox.Services.Authentication.WebAPI.Controllers.Authentication
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticationController(ICustomUserManager customUserManager,
        ICustomTokenManager customTokenManager, IValidator<LoginDto> loginDtoValidator) : Controller
    {
        #region [ Fields ]

        private readonly ICustomUserManager _customUserManager = customUserManager;

        private readonly ICustomTokenManager _customTokenManager = customTokenManager;

        private readonly IValidator<LoginDto> _loginDtoValidator = loginDtoValidator;

        #endregion

        #region [ JWT Operations ]

        [HttpPost]
        [Route("connect/token")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto userCredential)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(userCredential);
            if (!validationResult.IsValid)
            {
                throw new FluentValidationException(typeof(LoginDto), validationResult);
            }

            var (jwtToken, refreshToken) = await _customUserManager.Authenticate(userCredential);

            if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
            {
                throw new NoxAuthenticationApiException("Invalid login attempt", (int)HttpStatusCode.Unauthorized);
            }
            if (jwtToken == "I am a teapot")
            {
                throw new NoxAuthenticationApiException("I am a teapot", (int)HttpStatusCode.Locked);
            }

            return Ok(new { Token = jwtToken, RefreshToken = refreshToken });
        }

        [HttpGet]
        [Route("verify-token/{token}")]
        public IActionResult Verify(string token, string audience)
        {
            return Ok(_customTokenManager.VerifyToken(token, audience));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto model)
        {
            var userId = _customTokenManager.GetUserInfoByToken(model.Token);
            var storedRefreshToken = await _customUserManager.RetrieveStoredRefreshToken(userId);

            if (!_customTokenManager.VerifyRefreshToken(model.RefreshToken, storedRefreshToken))
            {
                throw new NoxAuthenticationApiException("", (int)HttpStatusCode.Unauthorized);
            }

            var newJwtToken = await _customTokenManager.CreateToken(userId, model.Audience);
            var newRefreshToken = _customTokenManager.CreateRefreshToken();
            await _customUserManager.SaveRefreshToken(userId, newRefreshToken);

            return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }

        #endregion
    }
}