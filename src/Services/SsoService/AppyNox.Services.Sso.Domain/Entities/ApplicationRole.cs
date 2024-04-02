using AppyNox.Services.Base.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Sso.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>, ICompanyScopedEntity, IHasCode
    {
        #region [ Relations ]

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid CompanyId { get; set; }

        public virtual Company Company { get; set; } = null!;

        #endregion
    }
}