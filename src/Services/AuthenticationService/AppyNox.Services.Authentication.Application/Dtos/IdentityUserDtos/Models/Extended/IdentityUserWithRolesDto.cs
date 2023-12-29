using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended
{
    /// <summary>
    /// Data transfer object for an identity user including associated roles.
    /// Inherits from IdentityUserDto.
    /// </summary>
    public class IdentityUserWithRolesDto : IdentityUserDto
    {
        #region [ Properties ]

        public IList<IdentityRoleDto>? Roles { get; set; }

        #endregion
    }
}