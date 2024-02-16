using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities;

public class LicenseEntity : IEntityTypeId, IHasCode
{
    #region [ Properties ]

    public LicenseId Id { get; set; } = new LicenseId(Guid.NewGuid());

    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string LicenseKey { get; set; } = string.Empty;

    public DateTime ExpirationDate { get; set; }

    public int MaxUsers { get; set; }

    public int MaxMacAddresses { get; set; }

    #endregion

    #region [ Relations ]

    public Guid? CompanyId { get; set; }

    public virtual ICollection<ApplicationUserLicenses>? ApplicationUserLicenses { get; set; }

    public ProductId ProductId { get; set; }

    public virtual ProductEntity Product { get; set; } = default!;

    #endregion

    #region [ IEntityTypeId ]

    Guid IEntityTypeId.GetTypedId => Id.Value;

    #endregion
}

public sealed record LicenseId(Guid Value) : IHasGuidId
{
    public Guid GetGuidValue() => Value;
}