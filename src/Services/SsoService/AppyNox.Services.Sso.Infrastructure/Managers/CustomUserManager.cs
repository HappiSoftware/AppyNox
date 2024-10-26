﻿using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Sso.Application.Interfaces.Authentication;
using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Sso.Infrastructure.Exceptions.Base;
using AppyNox.Services.Sso.Infrastructure.Localization;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AppyNox.Services.Sso.Infrastructure.Managers;

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
    /// <exception cref="NoxSsoInfrastructureException">Thrown when authentication fails.</exception>
    public async Task<(string jwtToken, string refreshToken)> Authenticate(LoginDto user)
    {
        var loggedUser = await _userManager.FindByNameAsync(user.UserName);
        if (loggedUser == null || string.IsNullOrEmpty(loggedUser.UserName))
        {
            throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, (int)NoxSsoInfrastructureExceptionCode.WrongCredentials, statusCode: (int)HttpStatusCode.BadRequest);
        }

        //validate credentials !! 3. parameter is for rememberme
        var result = await _signInManager.PasswordSignInAsync(loggedUser.UserName, user.Password, false, lockoutOnFailure: true);

        // Log in
        if (result.Succeeded)
        {
            var jwtToken = await customTokenManager.CreateToken(loggedUser.Id.ToString(), user.Audience);
            var refreshToken = customTokenManager.CreateRefreshToken();
            await SaveRefreshToken(loggedUser.Id.ToString(), refreshToken);
            return (jwtToken, refreshToken);
        }

        // Account is locked
        if (result.IsLockedOut) throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.Teapot, statusCode: (int)HttpStatusCode.Locked);
        else throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, (int)NoxSsoInfrastructureExceptionCode.WrongCredentials, (int)HttpStatusCode.BadRequest);
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
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, statusCode: (int)HttpStatusCode.BadRequest);

            await _userManager.RemoveAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken", refreshToken);
        }
        catch (Exception)
        {
            throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.RefreshTokenError, statusCode: (int)NoxSsoInfrastructureExceptionCode.RefreshToken);
        }
    }

    /// <summary>
    /// Retrieves the stored refresh token for a specified user.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <returns>The stored refresh token.</returns>
    /// <exception cref="NoxSsoInfrastructureException">Thrown when the user or refresh token is not found.</exception>
    public async Task<string> RetrieveStoredRefreshToken(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, (int)NoxSsoInfrastructureExceptionCode.WrongCredentials, (int)HttpStatusCode.BadRequest);

            return await _userManager.GetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken")
                ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.RefreshTokenNotFound, (int)NoxSsoInfrastructureExceptionCode.RefreshTokenNotFound, (int)HttpStatusCode.BadRequest);
        }
        catch (Exception)
        {
            throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.RefreshTokenNotFound, (int)NoxSsoInfrastructureExceptionCode.RefreshTokenNotFound, (int)HttpStatusCode.BadRequest);
        }
    }

    #endregion
}