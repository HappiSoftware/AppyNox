using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationUserDTOs.Models;

/// <summary>
/// Data transfer object representing an identity user.
/// </summary>
public class ApplicationUserDto : IHasCode
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion
}