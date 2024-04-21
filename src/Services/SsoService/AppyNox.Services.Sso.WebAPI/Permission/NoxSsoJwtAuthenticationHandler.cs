using AppyNox.Services.Sso.Application.Interfaces.Authentication;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;
using AppyNox.Services.Sso.WebAPI.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace AppyNox.Services.Sso.WebAPI.Permission;

internal partial class NoxSsoJwtAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ICustomTokenManager jwtTokenManager) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    #region [ Fields ]

    private readonly ICustomTokenManager _jwtTokenManager = jwtTokenManager;

    #endregion

    #region [ Protected Methods ]

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        var requestPath = Context.Request.Path.ToString();
        bool isConnectRequest = requestPath.EndsWith("/authentication/connect/token") || requestPath.EndsWith("/authentication/refresh") || requestPath.Equals("/api/health");
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null || isConnectRequest)
        {
            // Bypass authentication for this request
            return AuthenticateResult.NoResult();
        }

        string token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last()
            ?? throw new NoxSsoApiException(NoxSsoApiResourceService.NullToken, statusCode: (int)HttpStatusCode.Unauthorized);
        string audience = GetExpectedAudienceForRequest(Context.Request.Path.ToString());

        if (await _jwtTokenManager.VerifyToken(token, audience))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtToken.Claims;

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }

        throw new NoxSsoApiException(NoxSsoApiResourceService.InvalidToken, statusCode: (int)HttpStatusCode.Unauthorized);
    }

    protected static string GetExpectedAudienceForRequest(string requestPath)
    {
        return "AppyNox";

        // AppyNoxEmail is deprecated but didnt removed from the codebase. Lies down here for future references.
        if (EmailProviderPathRegex().IsMatch(requestPath))
        {
            return "AppyNoxEmail";
        }
        else
        {
            return "AppyNox";
        }
    }

    [GeneratedRegex(@"/api/v\d+(\.\d+)?/email-providers(/|$)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex EmailProviderPathRegex();

    #endregion
}