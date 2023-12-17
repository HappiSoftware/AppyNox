namespace AppyNox.Services.Authentication.Application.Dtos.AccountDtos.Models.Base
{
    public class LoginDto
    {
        #region [ Properties ]

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        #endregion
    }
}