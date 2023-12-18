using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base
{
    [IdentityUserDetailLevel(IdentityUserDataAccessDetailLevel.WithAllProperties)]
    public class IdentityUserWithAllPropertiesDto : IdentityUserWithRolesDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}