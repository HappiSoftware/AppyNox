using System.Text;

namespace AppyNox.Services.Authentication.WebAPI.Configuration
{
    /// <summary>
    /// Contains configuration settings for JWT (JSON Web Token).
    /// </summary>
    public class JwtConfiguration
    {
        #region [ Properties ]

        public string SecretKey { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public int TokenLifetimeMinutes { get; set; }

        #endregion

        #region [ Public Constructors ]

        public JwtConfiguration()
        {
        }

        public JwtConfiguration(string secretKey, string issuer, string audience, int tokenLifetimeMinutes = 1)
        {
            SecretKey = secretKey;
            Issuer = issuer;
            Audience = audience;
            TokenLifetimeMinutes = tokenLifetimeMinutes;
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Converts the SecretKey string to a byte array.
        /// </summary>
        /// <returns>A byte array representing the SecretKey.</returns>
        public byte[] GetSecretKeyBytes()
        {
            return Encoding.ASCII.GetBytes(SecretKey);
        }

        #endregion
    }
}