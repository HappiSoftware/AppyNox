using AppyNox.Services.Base.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AppyNox.Services.Coupon.Infrastructure.Authentication;

public class CouponAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ICouponTokenManager jwtTokenManager) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    #region [ Fields ]

    private readonly ICouponTokenManager _jwtTokenManager = jwtTokenManager;

    #endregion

    #region [ Protected Methods ]

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Endpoint? endpoint = Context.GetEndpoint();
        if (endpoint == null)
        {
            return AuthenticateResult.NoResult();
        }

        string requestPath = Context.Request.Path.ToString();
        string[] bypassAuthPaths = ["/api/health", "/swagger"];
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null || bypassAuthPaths.Contains(requestPath))
        {
            // Bypass authentication for this request
            return AuthenticateResult.NoResult();
        }

        string token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last()
            ?? throw new InfrastructureException("Token was null. Dev", 999);

        if (await _jwtTokenManager.VerifyToken(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtToken.Claims;

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }

        throw new InfrastructureException("Invalid token. Dev.", 999);
    }

    #endregion
}