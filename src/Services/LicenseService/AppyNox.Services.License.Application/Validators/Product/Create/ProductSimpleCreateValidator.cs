using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models;
using FluentValidation;

namespace AppyNox.Services.License.Application.Validators.Product.Create
{
    public class ProductSimpleCreateValidator : DtoValidatorBase<ProductCreateDto>
    {
        #region [ Public Constructors ]

        public ProductSimpleCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(5, 30).WithMessage("Name must be between 5 and 30 characters.");
        }

        #endregion
    }
}