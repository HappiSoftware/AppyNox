using AppyNox.Services.Base.Domain;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class CompanyEntity : EntityBase
    {
        #region [ Properties ]

        public string Name { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public virtual ICollection<ApplicationUser>? Users { get; set; }

        public virtual ICollection<ApplicationRole>? Roles { get; set; }

        #endregion
    }
}