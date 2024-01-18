namespace AppyNox.Services.Base.Application.Interfaces.Authentication
{
    public interface INoxTokenManager
    {
        #region [ Public Methods ]

        string GetUserInfoByToken(string token);

        bool VerifyToken(string token);

        #endregion
    }
}