using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Managers;
using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.WebAPI.UnitTest.Managers
{
    public class JwtTokenManagerUnitTest
    {
        #region [ Fields ]

        private readonly JwtTokenManager _jwtTokenManager;

        private readonly JwtConfiguration _jwtConfiguration;

        #endregion

        #region Public Constructors

        public JwtTokenManagerUnitTest()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"JwtSettings:AppyNox:Issuer", "YourIssuer"},
                {"JwtSettings:AppyNox:Audience", "YourAudience"},
                {"JwtSettings:AppyNox:SecretKey", "ThisIsCompletelyAValidSecretKeyOrMaybeNot"},
                {"JwtSettings:AppyNox:TokenLifetimeMinutes", "1"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
            _jwtConfiguration = new();
            configuration.GetSection("JwtSettings:AppyNox").Bind(_jwtConfiguration);

            _jwtTokenManager = new(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<RoleManager<ApplicationRole>>(), configuration);
        }

        #endregion

        #region [ Private Methods ]

        private string GenerateTestToken(bool expired = false)
        {
            var securityKey = new SymmetricSecurityKey(_jwtConfiguration.GetSecretKeyBytes());
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, "testUser") };

            var notBefore = expired ? DateTime.UtcNow.AddMinutes(-10) : DateTime.UtcNow;
            var expires = expired ? DateTime.UtcNow.AddMinutes(-5) : DateTime.UtcNow.AddMinutes(30);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                NotBefore = notBefore,
                SigningCredentials = credentials,
                Issuer = _jwtConfiguration.Issuer,
                Audience = _jwtConfiguration.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion

        #region [ Public Methods ]

        [Fact]
        public void VerifyToken_ShouldReturnTrue_WithValidToken()
        {
            // Arrange
            string token = GenerateTestToken();

            // Act
            bool result = _jwtTokenManager.VerifyToken(token, "AppyNox");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyToken_ShouldThrowException_WithExpiredToken()
        {
            // Arrange
            string token = GenerateTestToken(expired: true);

            // Act & Assert
            var exception = Assert.Throws<NoxAuthenticationApiException>(() => _jwtTokenManager.VerifyToken(token, "AppyNox"));
            Assert.Equal((int)HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        [Fact]
        public void VerifyToken_ShouldReturnFalse_WithInvalidToken()
        {
            // Arrange
            string token = "invalidTokenString";

            // Act
            bool result = _jwtTokenManager.VerifyToken(token, "AppyNox");

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}