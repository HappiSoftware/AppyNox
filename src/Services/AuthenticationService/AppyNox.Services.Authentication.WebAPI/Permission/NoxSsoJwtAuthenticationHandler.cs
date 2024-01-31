using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.WebAPI.Localization;
using AppyNox.Services.Base.API.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AppyNox.Services.Authentication.WebAPI.Permission
{
    internal class NoxSsoJwtAuthenticationHandlerOptions : AuthenticationSchemeOptions
    {
        #region [ Properties ]

        public string Audience { get; set; } = string.Empty;

        #endregion
    }

    internal class NoxSsoJwtAuthenticationHandler(
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
            var requestPath = Context.Request.Path.ToString();
            bool isConnectRequest = requestPath.EndsWith("/authentication/connect/token") || requestPath.EndsWith("/authentication/refresh") || requestPath.Equals("/api/health");
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null || isConnectRequest)
            {
                // Bypass authentication for this request
                return AuthenticateResult.NoResult();
            }

            string token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last()
                ?? throw new NoxSsoApiException(NoxSsoApiResourceService.NullToken, (int)HttpStatusCode.Unauthorized);
            string audience = Options.Audience;

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

            throw new NoxSsoApiException(NoxSsoApiResourceService.InvalidToken, (int)HttpStatusCode.Unauthorized);
        }

        #endregion
    }
}