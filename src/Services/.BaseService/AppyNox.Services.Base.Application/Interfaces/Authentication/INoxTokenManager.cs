namespace AppyNox.Services.Base.Application.Interfaces.Authentication
{
    public interface INoxTokenManager
    {
        #region [ Public Methods ]

        string GetUserInfoByToken(string token);

        Task<bool> VerifyToken(string token);

        #endregion
    }
}