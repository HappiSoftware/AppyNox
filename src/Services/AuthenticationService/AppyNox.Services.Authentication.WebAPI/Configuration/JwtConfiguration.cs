using System.Text;

namespace AppyNox.Services.Authentication.WebAPI.Configuration
{
    public class JwtConfiguration
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        public byte[] GetSecretKeyBytes()
        {
            return Encoding.ASCII.GetBytes(SecretKey);
        }
    }
}
