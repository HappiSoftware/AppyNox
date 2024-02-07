using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities
{
    public class ProductEntity : EntityBase, IEntityTypeId
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public virtual ICollection<LicenseEntity>? Licenses { get; set; }

        #endregion

        #region [ IEntityTypeId ]

        Guid IEntityTypeId.GetTypedId => Id;

        #endregion
    }
}