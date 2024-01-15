using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Validators.IdentityUser;
using AppyNox.Services.Authentication.Application.Validators.SharedRules;
using AppyNox.Services.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AppyNox.Services.Authentication.Application.UnitTest.FluentValidation
{
    public class IdentityUserCreateDtoValidatorUnitTest
    {
        #region [ Fields ]

        private const string _validPassword = "A1!bcde";

        private readonly Mock<IDatabaseChecks> _databaseChecksMock;

        private readonly Mock<IPasswordValidator<ApplicationUser>> _passwordValidator;

        private readonly IdentityUserCreateDtoValidator _validator;

        #endregion

        #region [ Public Constructors ]

        public IdentityUserCreateDtoValidatorUnitTest()
        {
            _databaseChecksMock = new();
            _passwordValidator = new();
            _passwordValidator.Setup(x => x.ValidateAsync(
                    It.IsAny<UserManager<ApplicationUser>>(),
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserManager<ApplicationUser> manager, ApplicationUser user, string password) =>
                    IsValidPassword(password) ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Invalid password." }));

            _databaseChecksMock.Setup(x => x.IsUsernameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _databaseChecksMock.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _validator = new IdentityUserCreateDtoValidator(_databaseChecksMock.Object, _passwordValidator.Object, It.IsAny<UserManager<ApplicationUser>>());
        }

        #endregion

        #region [ Public Methods ]

        [Theory]
        [InlineData("validUser", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public async Task Validate_Username_ShouldMatchExpected(string? username, bool expectedIsValid)
        {
            // Arrange
            var dto = new IdentityUserCreateDto { UserName = username!, Password = _validPassword, ConfirmPassword = _validPassword, Email = "test@happisoft.com" };

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData("user@example.com", true)]
        [InlineData("invalid-email", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public async Task Validate_Email_ShouldMatchExpected(string? email, bool expectedIsValid)
        {
            // Arrange
            var dto = new IdentityUserCreateDto { UserName = "TestUser", Password = _validPassword, ConfirmPassword = _validPassword, Email = email! };

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]

        // Valid password cases
        [InlineData("A1!bcde", true)] // Meets all requirements
        [InlineData("1!aAxyz", true)] // Meets all requirements in different order

        // Invalid password cases
        [InlineData("abcde", false)] // Too short, no digit, no non-alphanumeric, no uppercase
        [InlineData("A1!bc", false)] // Too short
        [InlineData("A1abcde", false)] // No non-alphanumeric
        [InlineData("1!abcde", false)] // No uppercase
        [InlineData("A1!BCDE", false)] // No lowercase
        [InlineData("A!bcdef", false)] // No digit
        [InlineData("123456", false)] // No uppercase, no lowercase, no non-alphanumeric
        [InlineData("ABCDEF", false)] // No digit, no lowercase, no non-alphanumeric
        [InlineData("abcdef", false)] // No digit, no uppercase, no non-alphanumeric
        [InlineData("A1bcde", false)] // No non-alphanumeric
        [InlineData("A!BCDE", false)] // No digit, no lowercase
        [InlineData("1!bcde", false)] // No uppercase
        public async Task Validate_Password_ShouldMatchExpected(string password, bool expectedIsValid)
        {
            // Arrange
            var dto = new IdentityUserCreateDto { UserName = "TestUser", Password = password, ConfirmPassword = password, Email = "test@happisoft.com" };

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData("Password123!", "Password123!", true)] // Matching passwords
        [InlineData("Password123!", "Different123!", false)] // Non-matching passwords
        public async Task Validate_PasswordConfirmPassword_ShouldMatch(string password, string confirmPassword, bool expectedIsValid)
        {
            // Arrange
            var dto = new IdentityUserCreateDto { UserName = "TestUser", Password = password, ConfirmPassword = confirmPassword, Email = "test@happisoft.com" };

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData("uniqueUser", "unique@example.com", true)] // Unique username and email
        [InlineData("existingUser", "unique@example.com", false)] // Existing username
        [InlineData("uniqueUser", "existing@example.com", false)] // Existing email
        public async Task Validate_DatabaseChecks_ShouldMatchExpected(string username, string email, bool expectedIsValid)
        {
            // Arrange
            var dto = new IdentityUserCreateDto { UserName = username, Email = email, Password = _validPassword, ConfirmPassword = _validPassword };
            _databaseChecksMock.Setup(x => x.IsUsernameUniqueAsync(username, It.IsAny<CancellationToken>())).ReturnsAsync(username != "existingUser");
            _databaseChecksMock.Setup(x => x.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(email != "existing@example.com");

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
        }

        #endregion

        #region [ Private Methods ]

        private static bool IsValidPassword(string password)
        {
            bool hasDigit = password.Any(char.IsDigit);
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasNonAlphanumeric = password.Any(c => !char.IsLetterOrDigit(c));
            bool hasRequiredLength = password.Length >= 6;

            return hasDigit && hasUpperCase && hasLowerCase && hasNonAlphanumeric && hasRequiredLength;
        }

        #endregion
    }
}