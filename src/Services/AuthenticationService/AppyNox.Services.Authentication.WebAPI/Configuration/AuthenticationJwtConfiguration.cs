using AppyNox.Services.Base.Core.Common;
using System.Text;

namespace AppyNox.Services.Authentication.WebAPI.Configuration
{
    /// <summary>
    /// Contains configuration settings for JWT (JSON Web Token).
    /// </summary>
    public class AuthenticationJwtConfiguration : JwtConfiguration
    {
        #region [ Properties ]

        public int TokenLifetimeMinutes { get; set; }

        #endregion

        #region [ Public Constructors ]

        public AuthenticationJwtConfiguration()
        {
        }

        public AuthenticationJwtConfiguration(string secretKey, string issuer, string audience, int tokenLifetimeMinutes = 1)
        {
            SecretKey = secretKey;
            Issuer = issuer;
            Audience = audience;
            TokenLifetimeMinutes = tokenLifetimeMinutes;
        }

        #endregion
    }
}