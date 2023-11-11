namespace AppyNox.Services.Authentication.Application.DTOs.RefreshTokenDTOs
{
    public class RefreshTokenDTO
    {
        #region [ Properties ]

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        #endregion
    }
}