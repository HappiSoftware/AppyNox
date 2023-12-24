using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.IntegrationTests.URIs
{
    public class ServiceURIs
    {
        #region [ Properties ]

        public string GatewayURI { get; set; } = string.Empty;

        public string CouponServiceURI { get; set; } = string.Empty;

        public string CouponServiceHealthURI { get; set; } = string.Empty;

        public string AuthenticationServiceURI { get; set; } = string.Empty;

        public string AuthenticationServiceHealthURI { get; set; } = string.Empty;

        #endregion
    }
}