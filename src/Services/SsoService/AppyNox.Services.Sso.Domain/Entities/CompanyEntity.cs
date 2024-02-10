using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Sso.Domain.Entities
{
    public class CompanyEntity : EntityBase, IEntityTypeId
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public virtual ICollection<ApplicationUser>? Users { get; set; }

        public virtual ICollection<ApplicationRole>? Roles { get; set; }

        #endregion

        #region [ IEntityTypeId ]

        Guid IEntityTypeId.GetTypedId => Id;

        #endregion
    }
}