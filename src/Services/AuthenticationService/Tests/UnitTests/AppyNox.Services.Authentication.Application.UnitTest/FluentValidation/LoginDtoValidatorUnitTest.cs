using AppyNox.Services.Authentication.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models;
using AppyNox.Services.Authentication.Application.Validators.Account;
using AppyNox.Services.Authentication.Application.Validators.IdentityUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.UnitTest.FluentValidation
{
    public class LoginDtoValidatorUnitTest
    {
        #region [ Fields ]

        private readonly LoginDtoValidator _validator = new();

        #endregion

        #region Public Methods

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("", "", false)]
        [InlineData(null, null, false)]
        public async Task Validate_UserNamePassword_ShouldMatchExpected(string? username, string? password, bool expectedIsValid)
        {
            // Arrange
            var dto = new LoginDto { UserName = username!, Password = password! };

            // Act
            var result = await _validator.ValidateAsync(dto);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
        }

        #endregion
    }
}