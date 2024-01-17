using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.Authentication;
using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AppyNox.Services.Authentication.WebAPI.Managers
{
    /// <summary>
    /// Manages the creation and validation of JWT tokens.
    /// </summary>
    public class JwtTokenManager(UserManager<ApplicationUser> userManager,
                                 RoleManager<ApplicationRole> roleManager,
                                 AuthenticationJwtConfiguration jwtConfiguration)
        : NoxTokenManager(jwtConfiguration), ICustomTokenManager
    {
        #region [ Fields ]

        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        private readonly AuthenticationJwtConfiguration _jwtConfiguration = jwtConfiguration;

        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        #endregion

        #region [ JWT Token ]

        /// <summary>
        /// Creates a JWT token for a specified user.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>A JWT token string.</returns>
        /// <exception cref="AuthenticationApiException">Thrown when user information is not found.</exception>
        public async Task<string> CreateToken(string userId)
        {
            List<Claim> claims = [];

            var user = await _userManager.FindByIdAsync(userId);

            IList<string> roles = await _userManager.GetRolesAsync(user ?? throw new AuthenticationApiException("Wrong Credentials", (int)HttpStatusCode.NotFound));

            claims.Add(new Claim(ClaimTypes.Name, user.Email ?? throw new AuthenticationApiException("Wrong Credentials", (int)HttpStatusCode.NotFound)));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            //create an empty list for userClaims
            IEnumerable<Claim> userClaims = Enumerable.Empty<Claim>();

            //fill userClaim with associated claims
            foreach (var item in roles)
            {
                var role = await _roleManager.FindByNameAsync(item)
                    ?? throw new AuthenticationApiException("Wrong Credentials", (int)HttpStatusCode.NotFound);
                if (role.Name == "SuperAdmin")
                {
                    claims.Add(new Claim("superadmin", "true"));
                }

                userClaims = userClaims.Concat(await _roleManager.GetClaimsAsync(role));
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
        /// Retrieves if user IsAdmin by a given JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>Bool value about IsAdmin</returns>
        /// <exception cref="AuthenticationApiException">Thrown when token is invalid or user information is not found.</exception>
        public bool GetIsAdmin(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) { return false; }

            var jwtToken = _tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken
                ?? throw new NoxApiException("Wrong Credentials", (int)HttpStatusCode.NotFound);

            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "admin");
            if (claim != null)
            {
                return claim.Value.Equals("true");
            }
            return false;
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