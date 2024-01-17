using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.Validators.License.Create;

namespace AppyNox.Services.License.Application.Validators.License.Update
{
    public class LicenseSimpleUpdateValidator : DtoValidatorBase<LicenseSimpleUpdateDto>
    {
        #region [ Public Constructors ]

        public LicenseSimpleUpdateValidator(LicenseSimpleCreateValidator validator)
        {
            RuleFor(o => o)
                .SetValidator(validator);
        }

        #endregion
    }
}