namespace AppyNox.Services.Authentication.WebAPI.Managers.Interfaces
{
    /// <summary>
    /// Defines the required functionalities for a custom token manager.
    /// </summary>
    public interface ICustomTokenManager
    {
        #region [ Public Methods ]

        string CreateRefreshToken();

        Task<string> CreateToken(string userId);

        string GetUserInfoByToken(string token);

        bool VerifyRefreshToken(string tokenToVerify, string storedToken);

        bool VerifyToken(string token);

        #endregion
    }
}