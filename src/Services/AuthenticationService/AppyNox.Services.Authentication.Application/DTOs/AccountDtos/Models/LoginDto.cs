namespace AppyNox.Services.Authentication.Application.DTOs.AccountDtos.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for user login information.
    /// </summary>
    public class LoginDto
    {
        #region [ Properties ]

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        #endregion
    }
}