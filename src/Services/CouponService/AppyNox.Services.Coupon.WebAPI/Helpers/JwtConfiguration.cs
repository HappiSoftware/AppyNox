using System.Text;

namespace AppyNox.Services.Coupon.WebAPI.Helpers;

public class JwtConfiguration
{
    #region [ Properties ]

    public string SecretKey { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    #endregion

    #region [ Public Methods ]

    public byte[] GetSecretKeyBytes()
    {
        return Encoding.ASCII.GetBytes(SecretKey);
    }

    #endregion
}