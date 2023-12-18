using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended
{
    [IdentityUserDetailLevel(IdentityUserDataAccessDetailLevel.WithAllRelations)]
    public class IdentityUserWithRolesDto : IdentityUserDto
    {
        #region [ Properties ]

        public IList<IdentityRoleDto>? Roles { get; set; }

        #endregion
    }
}