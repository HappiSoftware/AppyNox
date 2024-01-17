namespace AppyNox.Services.Base.Application.Interfaces.Authentication
{
    public interface INoxTokenManager
    {
        string GetUserInfoByToken(string token);
        bool VerifyToken(string token);
    }
}