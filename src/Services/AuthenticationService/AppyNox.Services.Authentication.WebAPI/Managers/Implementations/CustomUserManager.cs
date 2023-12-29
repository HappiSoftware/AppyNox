using AppyNox.Services.Authentication.Application.Dtos.AccountDtos.Models.Base;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Implementations
{
    public class CustomUserManager : ICustomUserManager
    {
        #region [ Fields ]

        private readonly ICustomTokenManager customTokenManager;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        #endregion

        #region [ Public Constructors ]

        public CustomUserManager(ICustomTokenManager customTokenManager, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.customTokenManager = customTokenManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #endregion

        #region [ Public Methods ]

        public async Task<(string jwtToken, string refreshToken)> Authenticate(LoginDto user)
        {
            var loggedUser = await _userManager.FindByNameAsync(user.UserName);
            if (loggedUser == null || string.IsNullOrEmpty(loggedUser.UserName))
            {
                throw new AuthenticationServiceException("Wrong credentials", (int)HttpStatusCode.BadRequest);
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
            if (result.IsLockedOut) throw new AuthenticationServiceException("I am a teapot", (int)HttpStatusCode.Locked);
            else throw new AuthenticationServiceException("Wrong credentials", (int)HttpStatusCode.BadRequest);
        }

        public async Task SaveRefreshToken(string userId, string refreshToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new AuthenticationServiceException("Wrong credentials", (int)HttpStatusCode.BadRequest);
                }
                await _userManager.RemoveAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken");
                await _userManager.SetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken", refreshToken);
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
                    throw new AuthenticationServiceException("Wrong credentials", (int)HttpStatusCode.BadRequest);
                }
                var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken");
                return refreshToken ?? throw new AuthenticationServiceException("No refresh token found. Please re-login to get a new one.", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                throw new AuthenticationServiceException("No refresh token found. Please re-login to get a new one.", (int)HttpStatusCode.BadRequest);
            }
        }

        #endregion
    }
}