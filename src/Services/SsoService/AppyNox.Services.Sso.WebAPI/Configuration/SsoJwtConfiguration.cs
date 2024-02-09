using AppyNox.Services.Base.Core.Common;
using System.Text;

namespace AppyNox.Services.Sso.WebAPI.Configuration
{
    /// <summary>
    /// Contains configuration settings for JWT (JSON Web Token).
    /// </summary>
    public class SsoJwtConfiguration : JwtConfiguration
    {
        #region [ Properties ]

        public int TokenLifetimeMinutes { get; set; }

        #endregion

        #region [ Public Constructors ]

        public SsoJwtConfiguration()
        {
        }

        #endregion
    }
}