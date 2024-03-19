using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities
{
    public class ApplicationUserLicenses
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        #endregion

        #region [ Relations ]

        public LicenseId LicenseId { get; set; } = default!;

        public virtual LicenseEntity License { get; set; } = default!;

        public virtual ICollection<ApplicationUserLicenseMacAddress>? MacAddresses { get; set; }

        #endregion
    }
}