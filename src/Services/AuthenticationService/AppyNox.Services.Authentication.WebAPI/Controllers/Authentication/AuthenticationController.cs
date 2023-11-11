using AppyNox.Services.Authentication.Application.Account;
using AppyNox.Services.Authentication.Application.DTOs.RefreshTokenDTOs;
using AppyNox.Services.Authentication.WebAPI.Helpers;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AutoWrapper.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.Authentication.WebAPI.Controllers.Authentication
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region [ Fields ]

        private readonly ICustomUserManager _customUserManager;

        private readonly ICustomTokenManager _customTokenManager;

        private readonly IValidator<LoginDTO> _loginDtoValidator;

        #endregion

        #region [ Public Constructors ]

        public AuthenticationController(ICustomUserManager customUserManager,
            ICustomTokenManager customTokenManager, IValidator<LoginDTO> loginDtoValidator)
        {
            _customUserManager = customUserManager;
            _customTokenManager = customTokenManager;
            _loginDtoValidator = loginDtoValidator;
        }

        #endregion

        #region [ JWT Operations ]

        [HttpPost]
        [Route("/connect/token")]
        public async Task<ApiResponse> Authenticate([FromBody] LoginDTO userCredential)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(userCredential);
            ValidationHandler.HandleValidationResult(ModelState, validationResult);

            var tokens = await _customUserManager.Authenticate(userCredential);

            if (string.IsNullOrEmpty(tokens.jwtToken) || string.IsNullOrEmpty(tokens.refreshToken))
            {
                throw new ApiProblemDetailsException("Invalid login attempt", 401);
            }
            if (tokens.jwtToken == "I am a teapot")
            {
                throw new ApiProblemDetailsException("I am a teapot", 418);
            }

            return new ApiResponse(new { Token = tokens.jwtToken, RefreshToken = tokens.refreshToken }, 200);
        }

        [HttpGet]
        [Route("/verifytoken/{token}")]
        public ApiResponse Verify(string token)
        {
            return new ApiResponse(_customTokenManager.VerifyToken(token), 200);
        }

        [HttpGet]
        [Route("/getuserinfo")]
        public ApiResponse GetUserInfoByToken(string token)
        {
            return new ApiResponse(_customTokenManager.GetUserInfoByToken(token), 200);
        }

        [HttpPost("refresh")]
        public async Task<ApiResponse> Refresh([FromBody] RefreshTokenDTO model)
        {
            var userId = _customTokenManager.GetUserInfoByToken(model.Token);
            var storedRefreshToken = await _customUserManager.RetrieveStoredRefreshToken(userId);

            if (!_customTokenManager.VerifyRefreshToken(model.RefreshToken, storedRefreshToken))
            {
                throw new ApiProblemDetailsException("", 401);
            }

            var newJwtToken = await _customTokenManager.CreateToken(userId);
            var newRefreshToken = _customTokenManager.CreateRefreshToken();
            await _customUserManager.SaveRefreshToken(userId, newRefreshToken);

            if (!await _customUserManager.SaveRefreshToken(userId, newRefreshToken))
            {
                throw new ApiException("Error saving new refresh token.");
            }

            return new ApiResponse(new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }

        #endregion
    }
}