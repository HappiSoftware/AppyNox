using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Sso.WebAPI.Managers;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Core.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AppyNox.Services.Sso.WebAPI.Localization;
using AppyNox.Services.Sso.WebAPI.Exceptions;

namespace AppyNox.Services.Sso.WebAPI.UnitTest.Managers
{
    public class JwtTokenManagerUnitTest
    {
        #region [ Fields ]

        private readonly JwtTokenManager _jwtTokenManager;

        private readonly JwtConfiguration _jwtConfiguration;

        #endregion

        #region [ Public Constructors ]

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

            var localizer = new Mock<IStringLocalizer>();
            localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

            var localizerFactory = new Mock<IStringLocalizerFactory>();
            localizerFactory.Setup(lf => lf.Create(typeof(NoxSsoApiResourceService))).Returns(localizer.Object);

            NoxSsoApiResourceService.Initialize(localizerFactory.Object);
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
        public async Task VerifyToken_ShouldReturnTrue_WithValidToken()
        {
            // Arrange
            string token = GenerateTestToken();

            // Act
            bool result = await _jwtTokenManager.VerifyToken(token, "AppyNox");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyToken_ShouldThrowException_WithExpiredToken()
        {
            // Arrange
            string token = GenerateTestToken(expired: true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NoxTokenExpiredException>(() => _jwtTokenManager.VerifyToken(token, "AppyNox"));
            Assert.Equal((int)HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        [Fact]
        public async Task VerifyToken_ShouldReturnFalse_WithInvalidToken()
        {
            // Arrange
            string token = "invalidTokenString";

            // Act
            var exception = await Assert.ThrowsAsync<NoxAuthenticationException>(() => _jwtTokenManager.VerifyToken(token, "AppyNox"));

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        #endregion
    }
}