using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models;
using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.WithRoles)]
    public class IdentityUserWithRolesDTO : IdentityUserDTO
    {
        #region [ Properties ]

        public IList<IdentityRoleDTO>? Roles { get; set; }

        #endregion
    }
}