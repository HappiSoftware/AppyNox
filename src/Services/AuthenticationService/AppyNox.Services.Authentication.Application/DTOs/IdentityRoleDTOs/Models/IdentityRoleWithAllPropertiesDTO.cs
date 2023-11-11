using AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs;
using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.WithAllProperties)]
    internal class IdentityRoleWithAllPropertiesDTO : IdentityRoleDTO
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        public IList<ClaimDTO>? Claims { get; set; }

        #endregion
    }
}