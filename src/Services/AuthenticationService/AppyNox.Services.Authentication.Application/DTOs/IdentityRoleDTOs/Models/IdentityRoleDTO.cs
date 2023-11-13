using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.Basic)]
    public class IdentityRoleDto
    {
        #region [ Properties ]

        public string Name { get; set; } = string.Empty;

        #endregion
    }
}