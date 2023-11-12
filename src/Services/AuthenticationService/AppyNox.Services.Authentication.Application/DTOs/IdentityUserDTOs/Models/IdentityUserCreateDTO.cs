namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models
{
    public class IdentityUserCreateDto
    {
        #region [ Properties ]

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        #endregion
    }
}