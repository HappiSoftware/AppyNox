using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities
{
    public class ProductEntity : EntityBase
    {
        #region [ Properties ]

        public string Name { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public virtual ICollection<LicenseEntity>? Licenses { get; set; }

        #endregion
    }
}