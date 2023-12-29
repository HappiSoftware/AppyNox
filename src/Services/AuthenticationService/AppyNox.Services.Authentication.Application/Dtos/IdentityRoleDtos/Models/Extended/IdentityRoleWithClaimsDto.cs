using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Extended
{
    /// <summary>
    /// Extended data transfer object for an identity role, including associated claims.
    /// Inherits from IdentityRoleDto.
    /// </summary>
    public class IdentityRoleWithClaimsDto : IdentityRoleDto
    {
        #region [ Properties ]

        public IList<ClaimDto>? Claims { get; set; }

        #endregion
    }
}