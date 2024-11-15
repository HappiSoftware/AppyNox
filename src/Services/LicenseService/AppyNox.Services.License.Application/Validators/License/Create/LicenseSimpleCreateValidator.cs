using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models;
using FluentValidation;

namespace AppyNox.Services.License.Application.Validators.License.Create
{
    public class LicenseSimpleCreateValidator : DtoValidatorBase<LicenseCreateDto>
    {
        #region [ Public Constructors ]

        public LicenseSimpleCreateValidator()
        {
            RuleFor(license => license.Description)
                .NotNull().NotEmpty().WithMessage("Description is mandatory")
                .MaximumLength(60).WithMessage("Description cannot be longer than 60 characters");

            RuleFor(license => license.LicenseKey)
                .NotNull().NotEmpty().WithMessage("License Key is mandatory");

            RuleFor(license => license.ExpirationDate)
                .NotNull().NotEmpty().WithMessage("Expiration Date is mandatory");

            RuleFor(license => license.MaxUsers)
                .NotNull().NotEmpty().WithMessage("Max Users is mandatory");

            RuleFor(license => license.MaxMacAddresses)
                .NotNull().NotEmpty().WithMessage("Max Mac Addresses is mandatory");

            RuleFor(license => license.ProductId)
                .NotNull().NotEmpty().WithMessage("ProductId is mandatory");
        }

        #endregion
    }
}