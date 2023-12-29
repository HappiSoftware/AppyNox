namespace AppyNox.Services.Authentication.Application.Dtos.AccountDtos.Models.Base
{
    /// <summary>
    /// Represents a data transfer object (DTO) for user login information.
    /// </summary>
    public class LoginDto
    {
        #region [ Properties ]

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        #endregion
    }
}