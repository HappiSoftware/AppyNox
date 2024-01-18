using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, ICompanyScopedEntity
    {
        #region [ Properties ]

        public string Code { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        #endregion

        #region [ Relations ]

        public Guid CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; } = null!;

        #endregion
    }
}