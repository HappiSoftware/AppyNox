namespace AppyNox.Services.Authentication.Application.Account
{
    public class LoginDTO
    {
        #region [ Properties ]

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        #endregion
    }
}