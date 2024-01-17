using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        #region [ Relations ]

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; } = null!;

        #endregion
    }
}