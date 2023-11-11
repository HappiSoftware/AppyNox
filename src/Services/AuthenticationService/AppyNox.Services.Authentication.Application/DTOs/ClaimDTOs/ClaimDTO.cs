namespace AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs
{
    public class ClaimDTO
    {
        #region [ Properties ]

        public string Issuer { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        #endregion
    }
}