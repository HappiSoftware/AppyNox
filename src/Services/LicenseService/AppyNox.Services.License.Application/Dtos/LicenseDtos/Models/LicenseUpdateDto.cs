using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.ValueObjects;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models
{
    public class LicenseUpdateDto : LicenseCreateDto
    {
        #region [ Properties ]

        public LicenseIdDto Id { get; set; } = default!;

        #endregion
    }
}