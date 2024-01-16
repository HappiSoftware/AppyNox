using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Managers.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.WebAPI.UnitTest.Managers
{
    public class JwtTokenManagerUnitTest
    {
        #region Fields

        private readonly AuthenticationJwtConfiguration _jwtConfiguration;

        private readonly JwtTokenManager _jwtTokenManager;

        #endregion

        #region Public Constructors

        public JwtTokenManagerUnitTest()
        {
            _jwtConfiguration = new("TotallySecretAndAccurateSecretKey", "HappiCorp", "HappyCustomers");
            _jwtTokenManager = new(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<RoleManager<IdentityRole>>(), _jwtConfiguration);
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
            bool result = _jwtTokenManager.VerifyToken(token);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyToken_ShouldThrowException_WithExpiredToken()
        {
            // Arrange
            string token = GenerateTestToken(expired: true);

            // Act & Assert
            var exception = Assert.Throws<AuthenticationServiceException>(() => _jwtTokenManager.VerifyToken(token));
            Assert.Equal((int)HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        [Fact]
        public void VerifyToken_ShouldReturnFalse_WithInvalidToken()
        {
            // Arrange
            string token = "invalidTokenString";

            // Act
            bool result = _jwtTokenManager.VerifyToken(token);

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}