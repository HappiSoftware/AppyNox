using AppyNox.Services.Authentication.Application.DTOs.AccountDtos.Models;

namespace AppyNox.Services.Authentication.Application.Interfaces.Authentication
{
    /// <summary>
    /// Defines the required functionalities for a custom user manager.
    /// </summary>
    public interface ICustomUserManager
    {
        #region [ Public Methods ]

        Task<(string jwtToken, string refreshToken)> Authenticate(LoginDto user);

        Task<string> RetrieveStoredRefreshToken(string userId);

        Task SaveRefreshToken(string userId, string refreshToken);

        #endregion
    }
}