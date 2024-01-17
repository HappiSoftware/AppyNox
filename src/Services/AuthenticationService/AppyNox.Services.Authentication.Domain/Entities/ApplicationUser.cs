using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        #region [ Properties ]

        public bool IsAdmin { get; set; }

        #endregion

        #region [ Relations ]

        public Guid CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; } = null!;

        #endregion
    }
}