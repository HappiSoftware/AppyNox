using AppyNox.Services.Authentication.Application.DTOs.ApplicationRoleDTOs.Models;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    /// <summary>
    /// Data transfer object for an identity user including associated roles.
    /// Inherits from IdentityUserDto.
    /// </summary>
    public class ApplicationUserWithRolesDto : ApplicationUserDto
    {
        #region [ Properties ]

        public IList<ApplicationRoleDto>? Roles { get; set; }

        #endregion
    }
}