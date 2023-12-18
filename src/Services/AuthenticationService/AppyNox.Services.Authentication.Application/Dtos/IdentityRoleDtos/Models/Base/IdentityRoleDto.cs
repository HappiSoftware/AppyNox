using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;
using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base
{
    [IdentityRoleDetailLevel(IdentityRoleDataAccessDetailLevel.Simple)]
    public class IdentityRoleDto : BaseDto
    {
        #region [ Properties ]

        public string Name { get; set; } = string.Empty;

        #endregion
    }
}