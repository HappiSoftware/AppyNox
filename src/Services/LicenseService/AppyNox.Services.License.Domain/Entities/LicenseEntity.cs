﻿using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Domain.Entities
{
    public class LicenseEntity : IEntityWithGuid
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string LicenseKey { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }

        public int MaxUsers { get; set; }

        public int MaxMacAddresses { get; set; }

        #endregion

        #region [ Relations ]

        public Guid? CompanyId { get; set; }

        public virtual ICollection<ApplicationUserLicenses>? ApplicationUserLicenses { get; set; }

        #endregion
    }
}