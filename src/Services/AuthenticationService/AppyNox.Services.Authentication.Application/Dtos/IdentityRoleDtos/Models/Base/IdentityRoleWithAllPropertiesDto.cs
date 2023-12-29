using AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base
{
    /// <summary>
    /// Extended data transfer object for an identity role, including all properties.
    /// </summary>
    internal class IdentityRoleWithAllPropertiesDto : IdentityRoleDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        public IList<ClaimDto>? Claims { get; set; }

        #endregion
    }
}