using AppyNox.Services.Base.Application.Interfaces.Authentication;

namespace AppyNox.Services.Authentication.Application.Interfaces.Authentication
{
    /// <summary>
    /// Defines the required functionalities for a custom token manager.
    /// </summary>
    public interface ICustomTokenManager : INoxTokenManager
    {
        #region [ Public Methods ]

        string CreateRefreshToken();

        Task<string> CreateToken(string userId);

        bool GetIsAdmin(string token);

        bool VerifyRefreshToken(string tokenToVerify, string storedToken);

        #endregion
    }
}