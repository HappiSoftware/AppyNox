using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base
{
    [LicenseDetailLevel(LicenseUpdateDetailLevel.Simple)]
    public class LicenseSimpleUpdateDto : LicenseSimpleCreateDto
    {
        #region [ Properties ]

        public LicenseIdDto Id { get; set; } = default!;

        #endregion
    }
}