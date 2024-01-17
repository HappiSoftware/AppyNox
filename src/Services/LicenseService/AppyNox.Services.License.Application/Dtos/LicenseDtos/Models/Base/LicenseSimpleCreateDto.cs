using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base
{
    [LicenseDetailLevel(LicenseCreateDetailLevel.Simple)]
    public class LicenseSimpleCreateDto : DtoBase
    {
        #region [ Properties ]

        public string Description { get; set; } = string.Empty;

        public string LicenseKey { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }

        public int MaxUsers { get; set; }

        public int MaxMacAddresses { get; set; }

        #endregion

        #region [ Relations ]

        public Guid ProductId { get; set; }

        #endregion
    }
}