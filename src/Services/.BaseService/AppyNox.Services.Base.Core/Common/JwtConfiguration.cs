using System.Text;

namespace AppyNox.Services.Base.Core.Common;

/// <summary>
/// Contains configuration settings for JWT (JSON Web Token).
/// </summary>
public class JwtConfiguration
{
    #region [ Properties ]

    public string SecretKey { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    #endregion

    #region [ Public Constructors ]

    public JwtConfiguration()
    {
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