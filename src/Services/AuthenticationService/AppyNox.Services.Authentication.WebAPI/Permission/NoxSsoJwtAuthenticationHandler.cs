using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AppyNox.Services.Authentication.WebAPI.Permission
{
    public class NoxSsoJwtAuthenticationHandlerOptions : AuthenticationSchemeOptions
    {
        #region [ Properties ]

        public string Audience { get; set; } = string.Empty;

        #endregion
    }

    public class NoxSsoJwtAuthenticationHandler(
     IOptionsMonitor<NoxSsoJwtAuthenticationHandlerOptions> options,
     ILoggerFactory logger,
     UrlEncoder encoder,
     ICustomTokenManager jwtTokenManager) : AuthenticationHandler<NoxSsoJwtAuthenticationHandlerOptions>(options, logger, encoder)
    {
        #region [ Fields ]

        private readonly ICustomTokenManager _jwtTokenManager = jwtTokenManager;

        #endregion

        #region [ Protected Methods ]

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                // Bypass authentication for this request
                return AuthenticateResult.NoResult();
            }

            string token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last()
                ?? throw new NoxAuthenticationApiException("Token is null!", (int)HttpStatusCode.Unauthorized);
            string audience = Options.Audience;

            if (_jwtTokenManager.VerifyToken(token, audience))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var claims = jwtToken.Claims;

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }

            throw new NoxAuthenticationApiException("Invalid token.", (int)HttpStatusCode.Unauthorized);
        }

        #endregion
    }
}