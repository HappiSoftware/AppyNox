using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.Basic)]
    public class IdentityUserDTO
    {
        #region [ Properties ]

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        #endregion
    }
}