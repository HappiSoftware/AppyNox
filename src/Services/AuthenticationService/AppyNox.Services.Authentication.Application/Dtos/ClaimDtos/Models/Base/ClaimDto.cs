namespace AppyNox.Services.Authentication.Application.Dtos.ClaimDtos.Models.Base
{
    public class ClaimDto
    {
        #region [ Properties ]

        public string Issuer { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        #endregion
    }
}