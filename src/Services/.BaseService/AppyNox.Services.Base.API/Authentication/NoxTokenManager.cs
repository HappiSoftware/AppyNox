using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Application.Interfaces.Authentication;
using AppyNox.Services.Base.Core.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace AppyNox.Services.Base.API.Authentication
{
    public class NoxTokenManager(JwtConfiguration jwtConfiguration) : INoxTokenManager
    {
        #region [ Fields ]

        private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration;

        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Validates a JWT token.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        /// <exception cref="NoxApiException">Thrown when token has expired.</exception>
        public async Task<bool> VerifyToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtConfiguration.GetSecretKeyBytes()),
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _jwtConfiguration.Audience,
                ValidIssuer = _jwtConfiguration.Issuer,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                _tokenHandler.ValidateToken(token, validationParameters, out SecurityToken sToken);
                return await Task.FromResult(true);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new NoxTokenExpiredException(NoxApiResourceService.ExpiredToken);
            }
            catch (Exception)
            {
                throw new NoxAuthenticationException(NoxApiResourceService.InvalidToken, (int)NoxApiExceptionCode.AuthenticationInvalidToken);
            }
        }

        /// <summary>
        /// Retrieves user information by validating a given JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>User information if token is valid.</returns>
        /// <exception cref="NoxApiException">Thrown when token is invalid or user information is not found.</exception>
        public string GetUserInfoByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return string.Empty;

            var jwtToken = _tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken
                ?? throw new NoxApiException(NoxApiResourceService.WrongCredentials, (int)HttpStatusCode.NotFound);

            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");
            if (claim != null) return claim.Value;
            return string.Empty;
        }

        #endregion
    }
}