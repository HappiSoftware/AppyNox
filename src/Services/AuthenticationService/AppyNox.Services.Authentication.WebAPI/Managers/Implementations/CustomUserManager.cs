using AppyNox.Services.Authentication.Application.Account;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Implementations
{
    public class CustomUserManager : ICustomUserManager
    {
        private readonly ICustomTokenManager customTokenManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public CustomUserManager(ICustomTokenManager customTokenManager, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.customTokenManager = customTokenManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<(string jwtToken, string refreshToken)> Authenticate(LoginDTO user)
        {
            IdentityUser loggedUser = await _userManager.FindByNameAsync(user.UserName) ?? throw new ApiProblemDetailsException("Wrong credentials", 400);
            if (loggedUser == null || string.IsNullOrEmpty(loggedUser.UserName))
            {
                throw new ApiProblemDetailsException("Wrong credentials", 400);
            }

            //validate credentials !! 3. parameter is for rememberme
            var result = await _signInManager.PasswordSignInAsync(loggedUser.UserName, user.Password, false, lockoutOnFailure: true);

            // Log in
            if (result.Succeeded)
            {
                var jwtToken = await customTokenManager.CreateToken(loggedUser.Id);
                var refreshToken = customTokenManager.CreateRefreshToken();
                await SaveRefreshToken(loggedUser.Id, refreshToken);
                return (jwtToken, refreshToken);
            }

            // Account is locked
            if (result.IsLockedOut) throw new ApiProblemDetailsException("I am a teapot", 418);

            else throw new ApiProblemDetailsException("Wrong credentials", 400);

        }

        public async Task<bool> SaveRefreshToken(string userId, string refreshToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ApiProblemDetailsException("Wrong credentials", 400);
                }
                await _userManager.RemoveAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken");
                await _userManager.SetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken", refreshToken);
                return true;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to save the refresh token. Please try again");
            }
        }

        public async Task<string> RetrieveStoredRefreshToken(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ApiProblemDetailsException("Wrong credentials", 400);
                }
                var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken");
                return refreshToken ?? throw new ApiProblemDetailsException("No refresh token found. Please re-login to get a new one.", 400);
            }
            catch (Exception)
            {
                throw new ApiProblemDetailsException("No refresh token found. Please re-login to get a new one.", 400);
            }
        }
    }
}
