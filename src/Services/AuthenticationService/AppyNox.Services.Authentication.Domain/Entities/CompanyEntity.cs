using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class CompanyEntity : EntityBase, IEntityWithGuid
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public virtual ICollection<ApplicationUser>? Users { get; set; }

        public virtual ICollection<ApplicationRole>? Roles { get; set; }

        #endregion
    }
}