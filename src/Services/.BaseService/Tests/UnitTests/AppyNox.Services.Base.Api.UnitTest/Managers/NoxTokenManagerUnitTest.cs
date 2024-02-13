using AppyNox.Services.Base.API.Authentication;
using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Core.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Api.UnitTest.Managers
{
    public class NoxTokenManagerUnitTest
    {
        #region [ Fields ]

        private readonly NoxTokenManager _noxTokenManager;

        private readonly JwtConfiguration _jwtConfiguration;

        #endregion

        #region [ Public Constructors ]

        public NoxTokenManagerUnitTest()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"JwtSettings:Issuer", "YourIssuer"},
                {"JwtSettings:Audience", "YourAudience"},
                {"JwtSettings:SecretKey", "ThisIsCompletelyAValidSecretKeyOrMaybeNot"},
                {"JwtSettings:TokenLifetimeMinutes", "1"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
            _jwtConfiguration = new();
            configuration.GetSection("JwtSettings").Bind(_jwtConfiguration);

            _noxTokenManager = new(_jwtConfiguration);

            var localizer = new Mock<IStringLocalizer>();
            localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

            var localizerFactory = new Mock<IStringLocalizerFactory>();
            localizerFactory.Setup(lf => lf.Create(typeof(NoxApiResourceService))).Returns(localizer.Object);

            NoxApiResourceService.Initialize(localizerFactory.Object);
        }

        #endregion

        #region [ Private Methods ]

        private string GenerateTestToken(bool expired = false)
        {
            var securityKey = new SymmetricSecurityKey(_jwtConfiguration.GetSecretKeyBytes());
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, "testUser") };

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
        public async Task VerifyToken_ShouldReturnTrue_WithValidToken()
        {
            // Arrange
            string token = GenerateTestToken();

            // Act
            bool result = await _noxTokenManager.VerifyToken(token);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyToken_ShouldThrowException_WithExpiredToken()
        {
            // Arrange
            string token = GenerateTestToken(expired: true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NoxTokenExpiredException>(() => _noxTokenManager.VerifyToken(token));
            Assert.Equal((int)HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        [Fact]
        public async Task VerifyToken_ShouldReturnFalse_WithInvalidToken()
        {
            // Arrange
            string token = "invalidTokenString";

            // Act
            var exception = await Assert.ThrowsAsync<NoxAuthenticationException>(() => _noxTokenManager.VerifyToken(token));

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        #endregion
    }
}