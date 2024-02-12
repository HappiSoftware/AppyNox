using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationRoleDTOs.Models;

/// <summary>
/// Data transfer object for creating a new identity role.
/// </summary>
public class ApplicationRoleCreateDto : IHasCode
{
    #region [ Properties ]

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}