using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities
{
    public class LicenseEntity : EntityBase
    {
        #region [ Properties ]

        public string Description { get; set; } = string.Empty;

        public string LicenseKey { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }

        public int MaxUsers { get; set; }

        public int MaxMacAddresses { get; set; }

        #endregion

        #region [ Relations ]

        public Guid? CompanyId { get; set; }

        public virtual ICollection<ApplicationUserLicenses>? ApplicationUserLicenses { get; set; }

        public Guid ProductId { get; set; }

        public virtual ProductEntity Product { get; set; } = default!;

        #endregion
    }
}