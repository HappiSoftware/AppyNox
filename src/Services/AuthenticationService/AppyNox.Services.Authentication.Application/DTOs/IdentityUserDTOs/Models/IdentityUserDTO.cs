using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.Basic)]
    public class IdentityUserDto
    {
        #region [ Properties ]

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        #endregion
    }
}