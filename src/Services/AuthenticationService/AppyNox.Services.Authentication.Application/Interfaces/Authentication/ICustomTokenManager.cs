using AppyNox.Services.Base.Application.Interfaces.Authentication;

namespace AppyNox.Services.Authentication.Application.Interfaces.Authentication
{
    /// <summary>
    /// Defines the required functionalities for a custom token manager.
    /// </summary>
    public interface ICustomTokenManager
    {
        #region [ Public Methods ]

        string CreateRefreshToken();

        Task<string> CreateToken(string userId, string audience);

        bool GetIsAdmin(string token);

        bool VerifyRefreshToken(string tokenToVerify, string storedToken);

        string GetUserInfoByToken(string token, string audience);

        Task<bool> VerifyToken(string token, string audience);

        #endregion
    }
}