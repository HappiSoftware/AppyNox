using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities;

public class LicenseEntity : AggregateRoot, IHasStronglyTypedId, IHasCode
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

    public ProductId ProductId { get; set; } = default!;

    public virtual ProductEntity Product { get; set; } = default!;

    #endregion

    #region [ IHasStronglyTypedId ]

    public Guid GetTypedId() => Id.Value;

    #endregion

    #region [ Constructors and Factories ]

    protected LicenseEntity()
    {
    }

    private LicenseEntity(Guid id, string code, string description, string licenseKey, DateTime expirationDate, int maxUsers, int maxMacAddresses, Guid? companyId, ProductId productId)
    {
        Id = new LicenseId(id);
        Code = code;
        Description = description;
        LicenseKey = licenseKey;
        ExpirationDate = expirationDate;
        MaxUsers = maxUsers;
        MaxMacAddresses = maxMacAddresses;
        CompanyId = companyId;
        ProductId = productId;
    }

    public static LicenseEntity Create(string code, string description, string licenseKey, DateTime expirationDate, int maxUsers, int maxMacAddresses, Guid? companyId, ProductId productId)
    {
        LicenseEntity entity = new(Guid.NewGuid(), code, description, licenseKey, expirationDate, maxUsers, maxMacAddresses, companyId, productId);
        return entity;
    }

    #endregion
}

public record LicenseId(Guid Value) : IHasGuidId
{
    public Guid GetGuidValue() => Value;
}