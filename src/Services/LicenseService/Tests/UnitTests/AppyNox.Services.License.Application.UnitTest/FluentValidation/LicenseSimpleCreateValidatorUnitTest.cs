using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.Validators.License.Create;

namespace AppyNox.Services.License.Application.UnitTest.FluentValidation
{
    public class LicenseSimpleCreateValidatorUnitTest
    {
        #region [ Fields ]

        private readonly LicenseSimpleCreateValidator _validator = new LicenseSimpleCreateValidator();

        #endregion

        #region [ Public Methods ]

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("ABCDEF", false)]
        [InlineData("ABCDE", true)]
        public async Task Validate_Code_ShouldMatchExpected(string? code, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.Code = code;

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("This is a longer description than allowed by the validator or not?", false)]
        [InlineData("Valid Description", true)]
        public async Task Validate_Description_ShouldMatchExpected(string? description, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.Description = description!;

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("ValidLicenseKey", true)]
        public async Task Validate_LicenseKey_ShouldMatchExpected(string? licenseKey, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.LicenseKey = licenseKey!;

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("2024-12-31", true)]
        public async Task Validate_ExpirationDate_ShouldMatchExpected(string? expirationDate, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.ExpirationDate = string.IsNullOrEmpty(expirationDate) ? default : DateTime.Parse(expirationDate);

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(10, true)]
        public async Task Validate_MaxUsers_ShouldMatchExpected(int maxUsers, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.MaxUsers = maxUsers;

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(5, true)]
        public async Task Validate_MaxMacAddresses_ShouldMatchExpected(int maxMacAddresses, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.MaxMacAddresses = maxMacAddresses;

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("fb26e285-ddfc-4d16-b6b7-529282d8ec51", true)]
        public async Task Validate_ProductId_ShouldMatchExpected(string? productId, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.ProductId = string.IsNullOrEmpty(productId) ? default : Guid.Parse(productId);

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        #endregion

        #region [ Private Methods ]

        private static LicenseSimpleCreateDto CreateValidDto()
        {
            return new LicenseSimpleCreateDto
            {
                Code = "ABCDE",
                Description = "Valid Description",
                LicenseKey = "ValidLicenseKey",
                ExpirationDate = DateTime.Now.AddYears(1),
                MaxUsers = 10,
                MaxMacAddresses = 5,
                ProductId = Guid.NewGuid()
            };
        }

        #endregion
    }
}