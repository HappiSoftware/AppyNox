namespace AppyNox.Services.Authentication.Application.Dtos.RefreshTokenDtos.Models.Base
{
    public class RefreshTokenDto
    {
        #region [ Properties ]

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        #endregion
    }
}