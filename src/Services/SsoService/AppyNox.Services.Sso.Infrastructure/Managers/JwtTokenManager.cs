using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Sso.Application.Interfaces.Authentication;
using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Sso.Infrastructure.Configuration;
using AppyNox.Services.Sso.Infrastructure.Exceptions;
using AppyNox.Services.Sso.Infrastructure.Exceptions.Base;
using AppyNox.Services.Sso.Infrastructure.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AppyNox.Services.Sso.Infrastructure.Managers;

/// <summary>
/// Manages the creation and validation of JWT tokens.
/// </summary>
public class JwtTokenManager(UserManager<ApplicationUser> userManager,
                         RoleManager<ApplicationRole> roleManager,
                         IConfiguration configuration)
    : ICustomTokenManager
{
    #region [ Fields ]

    private readonly UserManager<ApplicationUser> _userManager = userManager;

    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    private readonly IConfiguration _configuration = configuration;

    #endregion

    #region [ JWT Token ]

    /// <summary>
    /// Creates a JWT token for a specified user.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="audience">The user's audience.</param>
    /// <returns>A JWT token string.</returns>
    /// <exception cref="NoxSsoInfrastructureException">Thrown when user information is not found.</exception>
    public async Task<string> CreateToken(string userId, string audience)
    {
        List<Claim> claims = [];
        SsoJwtConfiguration jwtConfiguration = DetermineJwtConfigurationForAudience(audience);

        var user = await _userManager.FindByIdAsync(userId);

        IList<string> roles = await _userManager.GetRolesAsync(user ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, statusCode: (int)HttpStatusCode.NotFound));

        claims.Add(new Claim(ClaimTypes.Name, user.UserName ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, statusCode: (int)HttpStatusCode.NotFound)));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
        claims.Add(new Claim("company", user.CompanyId.ToString()));

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "admin"));
        }

        //create an empty list for userClaims
        IEnumerable<Claim> userClaims = [];

        //fill userClaim with associated claims
        foreach (var item in roles)
        {
            var role = await _roleManager.FindByNameAsync(item)
                ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, statusCode: (int)HttpStatusCode.NotFound);
            if (role.Name == "SuperAdmin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "superadmin"));

                // SAdmin will not use admin anyways, remove if added to prevent complications
                var adminClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "admin");
                if (adminClaim != null)
                {
                    claims.Remove(adminClaim);
                }
            }

            userClaims = userClaims.Concat(await _roleManager.GetClaimsAsync(role));
        }

        foreach (var item in userClaims)
        {
            claims.Add(new Claim(item.Type, item.Value));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtConfiguration.TokenLifetimeMinutes),
            Issuer = jwtConfiguration.Issuer,
            Audience = jwtConfiguration.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtConfiguration.GetSecretKeyBytes()), SecurityAlgorithms.HmacSha256Signature),
            IssuedAt = DateTime.UtcNow,
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Retrieves if user IsAdmin by a given JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>Bool value about IsAdmin</returns>
    /// <exception cref="NoxSsoInfrastructureException">Thrown when token is invalid or user information is not found.</exception>
    public bool GetIsAdmin(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) { return false; }

        var jwtToken = _tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken
            ?? throw new NoxSsoInfrastructureException("Wrong Credentials", statusCode: (int)HttpStatusCode.NotFound);

        var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "admin");
        if (claim != null)
        {
            return claim.Value.Equals("true");
        }
        return false;
    }

    public async Task<bool> VerifyToken(string token, string audience)
    {
        JwtConfiguration jwtConfiguration = DetermineJwtConfigurationForAudience(audience);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.GetSecretKeyBytes()),
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = jwtConfiguration.Audience,
            ValidIssuer = jwtConfiguration.Issuer,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            _tokenHandler.ValidateToken(token, validationParameters, out SecurityToken sToken);
            return await Task.FromResult(true);
        }
        catch (SecurityTokenExpiredException)
        {
            throw new NoxTokenExpiredException(NoxSsoInfrastructureResourceService.ExpiredToken);
        }
        catch (Exception)
        {
            throw new NoxAuthenticationException(NoxSsoInfrastructureResourceService.InvalidToken, (int)NoxSsoInfrastructureExceptionCode.AuthenticationInvalidToken);
        }
    }

    /// <summary>
    /// Retrieves user information by validating a given JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>User information if token is valid.</returns>
    /// <exception cref="NoxSsoInfrastructureException">Thrown when token is invalid or user information is not found.</exception>
    public string GetUserInfoByToken(string token, string audience)
    {
        var jwtToken = _tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken
            ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.WrongCredentials, statusCode: (int)HttpStatusCode.NotFound);

        var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");
        if (claim != null) return claim.Value;
        return string.Empty;
    }

    #endregion

    #region [ Refresh Token ]

    /// <summary>
    /// Creates a new refresh token.
    /// </summary>
    /// <returns>A new refresh token.</returns>
    public string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Verifies if a given refresh token matches the stored token.
    /// </summary>
    /// <param name="tokenToVerify">The refresh token to verify.</param>
    /// <param name="storedToken">The stored refresh token.</param>
    /// <returns>True if the tokens match; otherwise, false.</returns>
    public bool VerifyRefreshToken(string tokenToVerify, string storedToken)
    {
        return tokenToVerify == storedToken;
    }

    #endregion

    #region [ Private Methods ]

    private SsoJwtConfiguration DetermineJwtConfigurationForAudience(string audience)
    {
        var basePath = $"JwtSettings:{audience}";

        // Try to get the configuration section for the provided audience
        var jwtConfigSection = _configuration.GetSection(basePath);
        if (jwtConfigSection.Exists())
        {
            SsoJwtConfiguration jwtConfiguration = new();
            jwtConfigSection.Bind(jwtConfiguration);
            return jwtConfiguration;
        }
        else
        {
            throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.InvalidAudience, (int)NoxSsoInfrastructureExceptionCode.InvalidAudience, (int)HttpStatusCode.BadRequest);
        }
    }

    #endregion
}