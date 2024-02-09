namespace AppyNox.Services.Sso.Application.DTOs.ClaimDtos.Models
{
    /// <summary>
    /// Data transfer object representing a claim.
    /// </summary>
    public class ClaimDto
    {
        #region [ Properties ]

        public string Issuer { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        #endregion
    }
}