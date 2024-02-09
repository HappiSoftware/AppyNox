using AppyNox.Services.Sso.Application.DTOs.ClaimDtos.Models;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationRoleDTOs.Models
{
    /// <summary>
    /// Extended data transfer object for an identity role, including associated claims.
    /// Inherits from IdentityRoleDto.
    /// </summary>
    public class ApplicationRoleWithClaimsDto : ApplicationRoleDto
    {
        #region [ Properties ]

        public IList<ClaimDto>? Claims { get; set; }

        #endregion
    }
}