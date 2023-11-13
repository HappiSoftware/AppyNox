using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.WithAllProperties)]
    public class IdentityUserWithAllPropertiesDto : IdentityUserWithRolesDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}