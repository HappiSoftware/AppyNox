using AppyNox.Services.Base.API.Exceptions;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace AppyNox.Services.Base.API.Middleware;

/// <summary>
/// Middleware parsing correlation id and user id of the incoming request and storing them to NoxContext.
/// </summary>
public class NoxContextMiddleware(RequestDelegate next, INoxApiLogger logger)
{
    #region [ Fields ]

    private readonly RequestDelegate _next = next;

    private readonly INoxApiLogger _logger = logger;

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Invokes the middleware to check for a correlation ID in the incoming request.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/api/health"))
            {
                await _next(context);
                return;
            }

            string? correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId) || !Guid.TryParse(correlationId, out Guid parsedCorrelationId))
            {
                _logger.LogCritical(new MissingCorrelationIdException(), NoxApiResourceService.MissingCorrelationIdMessage);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(NoxApiResourceService.CorrelationIdIsRequired);
                return;
            }

            NoxContext.CorrelationId = Guid.Parse(correlationId);
            context.Response.Headers["X-Correlation-ID"] = correlationId;

            // Extracting and setting UserId and CompanyId from JWT token
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(' ').Last();
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    // userid
                    var jwtToken = handler.ReadJwtToken(token);
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                    if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid parsedUserId))
                    {
                        NoxContext.UserId = parsedUserId;
                    }

                    // companyid
                    var companyIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "company")?.Value;
                    if (!string.IsNullOrEmpty(companyIdClaim) && Guid.TryParse(companyIdClaim, out Guid parsedCompanyIdClaim))
                    {
                        NoxContext.CompanyId = parsedCompanyIdClaim;
                    }
                }
            }

            await _next(context);
        }
        finally
        {
            // Resetting the AsyncLocal values at the end of the request
            NoxContext.CorrelationId = Guid.Empty;
            NoxContext.UserId = Guid.Empty;
            NoxContext.CompanyId = Guid.Empty;
        }
    }

    #endregion
}