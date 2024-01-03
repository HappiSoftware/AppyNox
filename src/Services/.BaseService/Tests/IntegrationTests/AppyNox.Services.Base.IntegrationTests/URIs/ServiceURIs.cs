namespace AppyNox.Services.Base.IntegrationTests.URIs
{
    /// <summary>
    /// Holds the URIs for various services used in integration tests, facilitating easy access to these endpoints.
    /// </summary>
    public class ServiceURIs
    {
        #region [ Properties ]

        public string GatewayURI { get; set; } = string.Empty;

        public string CouponServiceURI { get; set; } = string.Empty;

        public string CouponServiceHealthURI { get; set; } = string.Empty;

        public string AuthenticationServiceURI { get; set; } = string.Empty;

        public string AuthenticationServiceHealthURI { get; set; } = string.Empty;

        public string LicenseServiceURI { get; set; } = string.Empty;

        public string LicenseServiceHealthURI { get; set; } = string.Empty;

        #endregion
    }
}