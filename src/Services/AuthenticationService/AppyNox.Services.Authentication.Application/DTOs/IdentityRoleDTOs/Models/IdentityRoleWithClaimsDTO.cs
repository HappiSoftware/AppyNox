using AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs;
using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.WithClaims)]
    public class IdentityRoleWithClaimsDTO : IdentityRoleDTO
    {
        #region [ Properties ]

        public IList<ClaimDTO>? Claims { get; set; }

        #endregion
    }
}