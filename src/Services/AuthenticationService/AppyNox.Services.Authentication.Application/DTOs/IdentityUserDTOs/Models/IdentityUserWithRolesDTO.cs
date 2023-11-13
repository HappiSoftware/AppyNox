using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.WithRoles)]
    public class IdentityUserWithRolesDto : IdentityUserDto
    {
        #region [ Properties ]

        public IList<IdentityRoleDto>? Roles { get; set; }

        #endregion
    }
}