using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Infrastructure.Encryption;
using AppyNox.Services.Base.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Base.Infrastructure.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public EncryptionService(IConfiguration configuration)
    {
        string? keyBase64 = configuration["Encryption:Key"];
        string? ivBase64 = configuration["Encryption:IV"];

        if (string.IsNullOrEmpty(keyBase64) || string.IsNullOrEmpty(ivBase64))
        {
            throw new InvalidOperationException("Encryption key or IV is not set in the configuration.");
        }

        _key = Convert.FromBase64String(keyBase64);
        _iv = Convert.FromBase64String(ivBase64);

        // Ensure key and IV lengths are suitable for AES
        if (_key.Length != 32) // 256-bit key for AES-256
        {
            throw new InvalidEncryptionException("Invalid key length. Key must be 256 bits (32 bytes).");
        }
        if (_iv.Length != 16) // 128-bit block size for AES
        {
            throw new InvalidEncryptionException("Invalid IV length. IV must be 128 bits (16 bytes).");
        }
    }

    public string EncryptString(string plainText)
    {
        return CryptoHelper.EncryptString(plainText, _key, _iv);
    }

    public string DecryptString(string cipherText)
    {
        return CryptoHelper.DecryptString(cipherText, _key, _iv);
    }
}