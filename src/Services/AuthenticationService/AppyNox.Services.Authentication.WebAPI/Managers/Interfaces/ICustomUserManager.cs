using AppyNox.Services.Authentication.Application.Dtos.AccountDtos.Models.Base;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Interfaces
{
    public interface ICustomUserManager
    {
        #region [ Public Methods ]

        Task<(string jwtToken, string refreshToken)> Authenticate(LoginDto user);

        Task<string> RetrieveStoredRefreshToken(string userId);

        Task<bool> SaveRefreshToken(string userId, string refreshToken);

        #endregion
    }
}