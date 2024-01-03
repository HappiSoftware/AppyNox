using AppyNox.Services.Authentication.Application.Dtos.AccountDtos.Models.Base;
using AppyNox.Services.Authentication.Infrastructure.Entities;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AppyNox.Services.Authentication.WebAPI.Managers.Implementations
{
    /// <summary>
    /// Custom implementation of user management functionalities.
    /// </summary>
    public class CustomUserManager : ICustomUserManager
    {
        #region [ Fields ]

        private readonly ICustomTokenManager customTokenManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region [ Public Constructors ]

        public CustomUserManager(ICustomTokenManager customTokenManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.customTokenManager = customTokenManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Authenticates a user and generates JWT and refresh tokens.
        /// </summary>
        /// <param name="user">The user's login information.</param>
        /// <returns>A tuple containing the JWT token and refresh token.</returns>
        /// <exception cref="AuthenticationServiceException">Thrown when authentication fails.</exception>
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

        /// <summary>
        /// Saves a refresh token for a specified user.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="refreshToken">The refresh token to save.</param>
        /// <exception cref="ApiException">Thrown when saving the refresh token fails.</exception>
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

        /// <summary>
        /// Retrieves the stored refresh token for a specified user.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>The stored refresh token.</returns>
        /// <exception cref="AuthenticationServiceException">Thrown when the user or refresh token is not found.</exception>
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