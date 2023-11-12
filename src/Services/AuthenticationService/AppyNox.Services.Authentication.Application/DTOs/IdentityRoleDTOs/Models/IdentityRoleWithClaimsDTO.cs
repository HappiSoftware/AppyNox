using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.WithClaims)]
    public class IdentityRoleWithClaimsDto : IdentityRoleDto
    {
        #region [ Properties ]

        public IList<ClaimDto>? Claims { get; set; }

        #endregion
    }
}