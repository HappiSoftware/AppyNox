namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    public class IdentityUserCreateDTO
    {
        #region [ Properties ]

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        #endregion
    }
}