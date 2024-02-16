using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Sso.Application.DTOs.RefreshTokenDtos.Models;
using AppyNox.Services.Sso.Application.Validators.Account;

namespace AppyNox.Services.Sso.Application.UnitTest.FluentValidation;

public class RefreshTokenDtoValidatorUnitTest
{
    #region [ Fields ]

    private readonly RefreshTokenDtoValidator _validator = new();

    #endregion

    #region [ Public Methods ]

    [Theory]
    [InlineData("test", "test", "test", true)]
    [InlineData("", "", "", false)]
    [InlineData(null, null, null, false)]
    public async Task Validate_RefreshTokenDto_ShouldMatchExpected(string? token, string? refreshToken, string? audience, bool expectedIsValid)
    {
        // Arrange
        var dto = new RefreshTokenDto { Token = token!, RefreshToken = refreshToken!, Audience = audience! };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    #endregion
}