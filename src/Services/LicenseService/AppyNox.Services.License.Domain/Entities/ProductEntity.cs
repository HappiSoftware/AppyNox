using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using System.ComponentModel.Design;

namespace AppyNox.Services.License.Domain.Entities;

public class ProductEntity : EntityBase, IHasStronglyTypedId, IHasCode
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

    Guid IHasStronglyTypedId.GetTypedId => Id.Value;

    #endregion

    #region [ Constructors and Factories ]

    private ProductEntity()
    {
    }

    private ProductEntity(Guid id, string name)
    {
        Id = new ProductId(id);
        Name = name;
    }

    public static ProductEntity Create(string name)
    {
        ProductEntity entity = new(Guid.NewGuid(), name);
        return entity;
    }

    #endregion
}

public sealed record ProductId(Guid Value) : IHasGuidId
{
    public Guid GetGuidValue() => Value;
}