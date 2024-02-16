using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities;

public class ProductEntity : EntityBase, IEntityTypeId, IHasCode
{
    #region [ Properties ]

    public ProductId Id { get; set; } = new ProductId(Guid.NewGuid());

    public string Name { get; set; } = string.Empty;

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public virtual ICollection<LicenseEntity>? Licenses { get; set; }

    #endregion

    #region [ IEntityTypeId ]

    Guid IEntityTypeId.GetTypedId => Id.Value;

    #endregion
}

public sealed record ProductId(Guid Value) : IHasGuidId
{
    public Guid GetGuidValue() => Value;
}