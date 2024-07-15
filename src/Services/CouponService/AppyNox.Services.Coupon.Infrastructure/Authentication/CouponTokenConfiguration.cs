using System.Text;

namespace AppyNox.Services.Coupon.Infrastructure.Authentication;

/// <summary>
/// This class is for testing the ability of InfrastructureServiceBuilder's multiple authentication scheme mechanism.
/// Do not use.
/// </summary>
public class CouponTokenConfiguration
{
    #region [ Properties ]

    public string SecretKey { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

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
