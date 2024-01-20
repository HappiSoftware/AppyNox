using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AutoWrapper.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AppyNox.Services.Authentication.Application.DTOs.RefreshTokenDtos.Models;
using AppyNox.Services.Authentication.Application.DTOs.AccountDtos.Models;

namespace AppyNox.Services.Authentication.WebAPI.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(ICustomUserManager customUserManager,
        ICustomTokenManager customTokenManager, IValidator<LoginDto> loginDtoValidator) : ControllerBase
    {
        #region [ Fields ]

        private readonly ICustomUserManager _customUserManager = customUserManager;

        private readonly ICustomTokenManager _customTokenManager = customTokenManager;

        private readonly IValidator<LoginDto> _loginDtoValidator = loginDtoValidator;

        #endregion

        #region [ JWT Operations ]

        [HttpPost]
        [Route("connect/token")]
        public async Task<ApiResponse> Authenticate([FromBody] LoginDto userCredential)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(userCredential);
            if (!validationResult.IsValid)
            {
                throw new FluentValidationException(typeof(LoginDto), validationResult);
            }

            var tokens = await _customUserManager.Authenticate(userCredential);

            if (string.IsNullOrEmpty(tokens.jwtToken) || string.IsNullOrEmpty(tokens.refreshToken))
            {
                throw new NoxAuthenticationApiException("Invalid login attempt", (int)HttpStatusCode.Unauthorized);
            }
            if (tokens.jwtToken == "I am a teapot")
            {
                throw new NoxAuthenticationApiException("I am a teapot", (int)HttpStatusCode.Locked);
            }

            return new ApiResponse(new { Token = tokens.jwtToken, RefreshToken = tokens.refreshToken }, 200);
        }

        [HttpGet]
        [Route("verifytoken/{token}")]
        public ApiResponse Verify(string token, string audience)
        {
            return new ApiResponse(_customTokenManager.VerifyToken(token, audience), 200);
        }

        [HttpGet]
        [Route("getuserinfo")]
        public ApiResponse GetUserInfoByToken(string token)
        {
            return new ApiResponse(_customTokenManager.GetUserInfoByToken(token), 200);
        }

        [HttpPost("refresh")]
        public async Task<ApiResponse> Refresh([FromBody] RefreshTokenDto model)
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

            return new ApiResponse(new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }

        #endregion
    }
}