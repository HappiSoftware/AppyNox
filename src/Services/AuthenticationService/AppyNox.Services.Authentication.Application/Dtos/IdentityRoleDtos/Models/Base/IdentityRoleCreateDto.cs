using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;
using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base
{
    [IdentityRoleDetailLevel(IdentityRoleCreateDetailLevel.Simple)]
    public class IdentityRoleCreateDto : BaseDto
    {
        #region [ Properties ]

        public string Name { get; set; } = string.Empty;

        #endregion
    }
}