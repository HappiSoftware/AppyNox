namespace AppyNox.Services.License.Domain.Entities
{
    public class ApplicationUserLicenses
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string MacAddress { get; set; } = string.Empty!;

        #endregion

        #region [ Relations ]

        public Guid LicenseId { get; set; }

        public virtual LicenseEntity License { get; set; } = null!;

        #endregion
    }
}