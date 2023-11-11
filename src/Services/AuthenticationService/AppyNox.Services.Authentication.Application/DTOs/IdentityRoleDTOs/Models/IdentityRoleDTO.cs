using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.Basic)]
    public class IdentityRoleDTO
    {
        #region [ Properties ]

        public string Name { get; set; } = string.Empty;

        #endregion
    }
}