using AppyNox.Services.Sso.Application.Interfaces.Authentication;
using AppyNox.Services.Sso.Infrastructure.Exceptions.Base;
using AppyNox.Services.Sso.Infrastructure.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AppyNox.Services.Sso.Infrastructure.Authentication;

public class NoxSsoJwtAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
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
            ?? throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.NullToken, statusCode: (int)HttpStatusCode.Unauthorized);

        if (await _jwtTokenManager.VerifyToken(token, "AppyNox"))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtToken.Claims;

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }

        throw new NoxSsoInfrastructureException(NoxSsoInfrastructureResourceService.InvalidToken, statusCode: (int)HttpStatusCode.Unauthorized);
    }

    #endregion
}