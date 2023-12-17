using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base
{
    [IdentityRoleDetailLevel(IdentityRoleDataAccessDetailLevel.WithAllProperties)]
    internal class IdentityRoleWithAllPropertiesDto : IdentityRoleDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        public IList<ClaimDto>? Claims { get; set; }

        #endregion
    }
}