using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities
{
    public class ApplicationUserLicenseMacAddress : IEntityTypeId
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string MacAddress { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public Guid ApplicationUserLicenseId { get; set; }

        public virtual ApplicationUserLicenses ApplicationUserLicense { get; set; } = null!;

        #endregion

        #region [ IEntityTypeId ]

        Guid IEntityTypeId.GetTypedId => Id;

        #endregion
    }
}