using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;

[LicenseDetailLevel(LicenseCreateDetailLevel.Simple)]
public class LicenseSimpleCreateDto : IHasCode
{
    #region [ Properties ]

    public string Description { get; set; } = string.Empty;

    public string LicenseKey { get; set; } = string.Empty;

    public DateTime ExpirationDate { get; set; }

    public int MaxUsers { get; set; }

    public int MaxMacAddresses { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public Guid ProductId { get; set; }

    #endregion
}