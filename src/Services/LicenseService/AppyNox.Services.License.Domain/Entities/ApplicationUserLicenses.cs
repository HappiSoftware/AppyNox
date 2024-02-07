using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities
{
    public class ApplicationUserLicenses : IEntityTypeId
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        #endregion

        #region [ Relations ]

        public Guid LicenseId { get; set; }

        public virtual LicenseEntity License { get; set; } = default!;

        public virtual ICollection<ApplicationUserLicenseMacAddress>? MacAddresses { get; set; }

        #endregion

        #region [ IEntityTypeId ]

        Guid IEntityTypeId.GetTypedId => Id;

        #endregion
    }
}