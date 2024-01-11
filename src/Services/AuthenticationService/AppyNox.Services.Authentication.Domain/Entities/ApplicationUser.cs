using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        #region [ Properties ]

        public Guid CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; } = null!;

        #endregion
    }
}