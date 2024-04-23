namespace AppyNox.Services.Base.Application.Interfaces.Encryption;

public interface IEncryptionService
{
    string EncryptString(string plainText);
    string DecryptString(string cipherText);
}