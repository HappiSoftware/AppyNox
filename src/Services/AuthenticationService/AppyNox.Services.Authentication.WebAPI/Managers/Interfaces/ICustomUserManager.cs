using AppyNox.Services.Authentication.Application.Dtos.AccountDtos.Models.Base;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Interfaces
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