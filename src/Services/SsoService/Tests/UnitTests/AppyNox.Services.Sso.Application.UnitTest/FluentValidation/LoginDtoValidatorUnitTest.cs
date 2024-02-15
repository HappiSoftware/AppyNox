using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Sso.Application.Validators.Account;

namespace AppyNox.Services.Sso.Application.UnitTest.FluentValidation;

public class LoginDtoValidatorUnitTest
{
    #region [ Fields ]

    private readonly LoginDtoValidator _validator = new();

    #endregion

    #region [ Public Methods ]

    [Theory]
    [InlineData("test", "test", true)]
    [InlineData("", "", false)]
    [InlineData(null, null, false)]
    public async Task Validate_UserNamePassword_ShouldMatchExpected(string? username, string? password, bool expectedIsValid)
    {
        // Arrange
        var dto = new LoginDto { UserName = username!, Password = password!, Audience = "audience" };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("test", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public async Task Validate_Audience_ShouldMatchExpected(string? audience, bool expectedIsValid)
    {
        // Arrange
        var dto = new LoginDto { UserName = "test", Password = "test", Audience = audience! };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    #endregion
}