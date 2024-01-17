using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        #region [ Relations ]

        public Guid CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; } = null!;

        #endregion
    }
}