using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Application.Validators.Product.Create;

namespace AppyNox.Services.License.Application.UnitTest.FluentValidation
{
    public class ProductSimpleCreateValidatorUnitTest
    {
        #region [ Fields ]

        private readonly ProductSimpleCreateValidator _validator = new ProductSimpleCreateValidator();

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
            dto.Code = code!;

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("ValidName", true)]
        [InlineData("Shor", false)]
        [InlineData("ThisIsAReallyLongNameThatExceedsTheMaximumLengthAllowed", false)]
        public async Task Validate_Name_ShouldMatchExpected(string? name, bool expectedIsValid)
        {
            var dto = CreateValidDto();
            dto.Name = name;

            var result = await _validator.ValidateAsync(dto);

            Assert.Equal(expectedIsValid, result.IsValid);
        }

        #endregion

        #region [ Private Methods ]

        private static ProductSimpleCreateDto CreateValidDto()
        {
            return new ProductSimpleCreateDto
            {
                Name = "ValidName",
                Code = "PROD1"
            };
        }

        #endregion
    }
}