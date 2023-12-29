using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Extended
{
    public class IdentityRoleWithClaimsDto : IdentityRoleDto
    {
        #region [ Properties ]

        public IList<ClaimDto>? Claims { get; set; }

        #endregion
    }
}