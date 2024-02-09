namespace AppyNox.Services.Sso.Application.DTOs.RefreshTokenDtos.Models
{
    /// <summary>
    /// Data transfer object representing a refresh token.
    /// </summary>
    public class RefreshTokenDto
    {
        #region [ Properties ]

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        #endregion
    }
}