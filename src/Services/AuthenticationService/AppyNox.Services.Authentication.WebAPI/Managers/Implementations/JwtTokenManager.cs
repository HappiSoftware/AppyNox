using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Implementations
{
    public class JwtTokenManager(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        JwtConfiguration jwtConfiguration, IConfiguration configuration) : ICustomTokenManager
    {
        #region [ Fields ]

        private readonly UserManager<IdentityUser> _userManager = userManager;

        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration;

        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        private readonly IConfiguration _configuration = configuration;

        #endregion

        #region [ JWT Token ]

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
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:TokenLifetimeMinutes")),
                Issuer = _jwtConfiguration.Issuer,
                Audience = _jwtConfiguration.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtConfiguration.GetSecretKeyBytes()), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.UtcNow,
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

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

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public bool VerifyRefreshToken(string tokenToVerify, string storedToken)
        {
            return tokenToVerify == storedToken;
        }

        #endregion
    }
}