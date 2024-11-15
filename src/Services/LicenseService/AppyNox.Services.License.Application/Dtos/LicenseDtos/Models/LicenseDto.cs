using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models
{
    public class LicenseDto : LicenseCreateDto, IAuditDto
    {
        #region [ IAuditDto ]

        public AuditInformation AuditInformation { get; set; } = default!;

        #endregion
    }
}