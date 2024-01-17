using AppyNox.Services.Authentication.Application.DTOs.ClaimDtos.Models;

namespace AppyNox.Services.Authentication.Application.DTOs.ApplicationRoleDTOs.Models
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