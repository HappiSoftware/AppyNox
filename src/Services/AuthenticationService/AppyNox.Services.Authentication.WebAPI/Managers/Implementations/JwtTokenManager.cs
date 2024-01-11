using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Implementations
{
    /// <summary>
    /// Manages the creation and validation of JWT tokens.
    /// </summary>
    public class JwtTokenManager(UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 JwtConfiguration jwtConfiguration) : ICustomTokenManager
    {
        #region [ Fields ]

        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration;

        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        #endregion

        #region [ JWT Token ]

        /// <summary>
        /// Creates a JWT token for a specified user.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>A JWT token string.</returns>
        /// <exception cref="AuthenticationServiceException">Thrown when user information is not found.</exception>
        public async Task<string> CreateToken(string userId)
        {
            List<Claim> claims = [];

            var user = await _userManager.FindByIdAsync(userId);

            IList<string> roles = await _userManager.GetRolesAsync(user ?? throw new AuthenticationServiceException("Wrong Credentials", (int)HttpStatusCode.NotFound));

            claims.Add(new Claim(ClaimTypes.Name, user.Email ?? throw new AuthenticationServiceException("Wrong Credentials", (int)HttpStatusCode.NotFound)));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            //create an empty list for userClaims
            IEnumerable<Claim> userClaims = Enumerable.Empty<Claim>();

            //fill userClaim with associated claims
            foreach (var item in roles)
            {
                var role = await _roleManager.FindByNameAsync(item);
                userClaims = userClaims.Concat(await _roleManager.GetClaimsAsync(role ?? throw new AuthenticationServiceException("Wrong Credentials", (int)HttpStatusCode.NotFound)));
            }

            foreach (var item in userClaims)
            {
                claims.Add(new Claim("claim", item.Value));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.TokenLifetimeMinutes),
                Issuer = _jwtConfiguration.Issuer,
                Audience = _jwtConfiguration.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtConfiguration.GetSecretKeyBytes()), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.UtcNow,
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Retrieves user information by validating a given JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>User information if token is valid.</returns>
        /// <exception cref="AuthenticationServiceException">Thrown when token is invalid or user information is not found.</exception>
        public string GetUserInfoByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return string.Empty;

            var jwtToken = _tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new AuthenticationServiceException("Wrong Credentials", (int)HttpStatusCode.NotFound);
            }

            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");
            if (claim != null) return claim.Value;
            return string.Empty;
        }

        /// <summary>
        /// Validates a JWT token.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        /// <exception cref="AuthenticationServiceException">Thrown when token has expired.</exception>
        public bool VerifyToken(string token)
        {
            SecurityToken securityToken;

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
                _tokenHandler.ValidateToken(token, validationParameters, out securityToken);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new AuthenticationServiceException("Token has expired", (int)HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                return false;
            }

            return securityToken != null;
        }

        #endregion

        #region [ Refresh Token ]

        /// <summary>
        /// Creates a new refresh token.
        /// </summary>
        /// <returns>A new refresh token.</returns>
        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Verifies if a given refresh token matches the stored token.
        /// </summary>
        /// <param name="tokenToVerify">The refresh token to verify.</param>
        /// <param name="storedToken">The stored refresh token.</param>
        /// <returns>True if the tokens match; otherwise, false.</returns>
        public bool VerifyRefreshToken(string tokenToVerify, string storedToken)
        {
            return tokenToVerify == storedToken;
        }

        #endregion
    }
}