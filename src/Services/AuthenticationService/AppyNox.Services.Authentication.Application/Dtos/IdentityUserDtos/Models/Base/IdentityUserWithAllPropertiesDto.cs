using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base
{
    /// <summary>
    /// Extended data transfer object for an identity user, including all properties and roles.
    /// Inherits from IdentityUserWithRolesDto.
    /// </summary>
    public class IdentityUserWithAllPropertiesDto : IdentityUserWithRolesDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}