using AppyNox.Services.Base.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Sso.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, ICompanyScopedEntity, IHasCode
    {
        #region [ Properties ]

        public string Code { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public Guid CompanyId { get; set; }

        public virtual Company Company { get; set; } = null!;

        #endregion
    }
}