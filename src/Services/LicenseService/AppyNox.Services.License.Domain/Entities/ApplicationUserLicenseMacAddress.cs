namespace AppyNox.Services.License.Domain.Entities
{
    public class ApplicationUserLicenseMacAddress
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string MacAddress { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public Guid ApplicationUserLicenseId { get; set; }

        public virtual ApplicationUserLicenses ApplicationUserLicense { get; set; } = null!;

        #endregion
    }
}