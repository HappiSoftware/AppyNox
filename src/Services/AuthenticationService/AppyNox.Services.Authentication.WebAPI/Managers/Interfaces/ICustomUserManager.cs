using AppyNox.Services.Authentication.Application.Account;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Interfaces
{
    public interface ICustomUserManager
    {
        Task<(string jwtToken, string refreshToken)> Authenticate(LoginDTO user);
        Task<string> RetrieveStoredRefreshToken(string userId);
        Task<bool> SaveRefreshToken(string userId, string refreshToken);
    }
}
