using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.WithAllProperties)]
    internal class IdentityRoleWithAllPropertiesDto : IdentityRoleDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        public IList<ClaimDto>? Claims { get; set; }

        #endregion
    }
}