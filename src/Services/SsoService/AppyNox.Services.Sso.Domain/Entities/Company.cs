using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Sso.Domain.Entities;

public class Company : IHasCode
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public virtual ICollection<ApplicationUser>? Users { get; set; }

    public virtual ICollection<ApplicationRole>? Roles { get; set; }

    public virtual ICollection<EmailProvider> EmailProviders { get; set; } = [];

    #endregion
}