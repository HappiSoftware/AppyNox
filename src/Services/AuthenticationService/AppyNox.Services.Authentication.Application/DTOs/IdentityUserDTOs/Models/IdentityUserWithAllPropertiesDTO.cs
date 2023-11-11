using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.WithAllProperties)]
    public class IdentityUserWithAllPropertiesDTO : IdentityUserWithRolesDTO
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}