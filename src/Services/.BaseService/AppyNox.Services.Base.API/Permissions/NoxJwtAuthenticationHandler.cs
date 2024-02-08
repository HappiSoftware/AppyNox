﻿using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AppyNox.Services.Base.API.Permissions
{
    public class NoxJwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        INoxTokenManager jwtTokenManager) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        #region [ Fields ]

        private readonly INoxTokenManager _jwtTokenManager = jwtTokenManager;

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
            var bypassAuthPaths = new[] { "/api/health", "/swagger" };
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null || bypassAuthPaths.Contains(requestPath))
            {
                // Bypass authentication for this request
                return AuthenticateResult.NoResult();
            }

            string token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last()
                ?? throw new NoxAuthenticationException(NoxApiResourceService.NullToken, (int)NoxApiExceptionCode.AuthenticationNullToken);

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

            throw new NoxAuthenticationException(NoxApiResourceService.InvalidToken, (int)NoxApiExceptionCode.AuthenticationInvalidToken);
        }

        #endregion
    }
}