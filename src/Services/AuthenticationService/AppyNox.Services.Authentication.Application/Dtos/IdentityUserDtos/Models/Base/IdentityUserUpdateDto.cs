using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base
{
    [IdentityUserDetailLevel(IdentityUserUpdateDetailLevel.Simple)]
    public class IdentityUserUpdateDto : IdentityUserCreateDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}