using AppyNox.Services.Base.Application.Validators;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models;
using AppyNox.Services.License.Application.Validators.License.Create;

namespace AppyNox.Services.License.Application.Validators.License.Update
{
    public class LicenseSimpleUpdateValidator : DtoValidatorBase<LicenseUpdateDto>
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