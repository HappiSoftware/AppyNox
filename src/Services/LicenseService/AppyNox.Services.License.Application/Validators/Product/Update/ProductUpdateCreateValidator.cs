using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using FluentValidation;

namespace AppyNox.Services.License.Application.Validators.Product.Create
{
    public class ProductUpdateCreateValidator : DtoValidatorBase<ProductSimpleUpdateDto>
    {
        #region [ Public Constructors ]

        public ProductUpdateCreateValidator(ProductSimpleCreateValidator validator)
        {
            RuleFor(o => o)
                .SetValidator(validator);
        }

        #endregion
    }
}