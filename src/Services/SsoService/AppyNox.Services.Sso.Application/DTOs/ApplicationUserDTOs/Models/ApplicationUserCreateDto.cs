using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationUserDTOs.Models;

/// <summary>
/// Data transfer object for creating a new identity user.
/// Contains user credentials and personal information.
/// </summary>
public class ApplicationUserCreateDto : IHasCode
{
    #region [ Properties ]

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string LicenseKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    #endregion

    #region [ IHasCode ]

    public string Code { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public Guid CompanyId { get; set; }

    #endregion
}