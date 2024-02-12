using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationRoleDTOs.Models;

/// <summary>
/// Data transfer object representing an identity role.
/// </summary>
public class ApplicationRoleDto : IHasCode
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}