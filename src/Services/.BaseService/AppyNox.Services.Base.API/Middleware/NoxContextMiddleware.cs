using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace AppyNox.Services.Base.API.Middleware
{
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

                // Extracting and setting UserId from JWT token
                var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(' ').Last();
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwtToken = handler.ReadJwtToken(token);
                        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                        if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid parsedUserId))
                        {
                            NoxContext.UserId = parsedUserId;
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
            }
        }

        #endregion
    }
}