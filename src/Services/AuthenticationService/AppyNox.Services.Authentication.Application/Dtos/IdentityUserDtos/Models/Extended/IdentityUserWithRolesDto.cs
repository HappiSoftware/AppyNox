using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended
{
    public class IdentityUserWithRolesDto : IdentityUserDto
    {
        #region [ Properties ]

        public IList<IdentityRoleDto>? Roles { get; set; }

        #endregion
    }
}