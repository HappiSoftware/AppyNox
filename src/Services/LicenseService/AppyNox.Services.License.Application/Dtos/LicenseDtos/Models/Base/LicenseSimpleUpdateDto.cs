using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base
{
    [LicenseDetailLevel(LicenseUpdateDetailLevel.Simple)]
    public class LicenseSimpleUpdateDto : LicenseSimpleCreateDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}