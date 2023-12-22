using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.IntegrationTests.URIs
{
    public static class ServiceURIs
    {
        #region [ Fields ]

        public const string GatewayURI = "https://localhost:7000/";

        public const string CouponServiceURI = $"coupon-service";

        public const string CouponServiceHealthURI = $"{CouponServiceURI}/health";

        public const string AuthenticationServiceURI = $"authentication-service";

        public const string AuthenticationServiceHealthURI = $"{AuthenticationServiceURI}/health";

        #endregion
    }
}