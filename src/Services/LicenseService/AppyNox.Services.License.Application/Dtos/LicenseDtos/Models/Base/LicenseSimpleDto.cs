using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base
{
    [LicenseDetailLevel(LicenseDataAccessDetailLevel.Simple)]
    public class LicenseSimpleDto : LicenseSimpleCreateDto
    {
    }
}