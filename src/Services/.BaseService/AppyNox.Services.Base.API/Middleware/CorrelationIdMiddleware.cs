using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AppyNox.Services.Base.API.Middleware
{
    /// <summary>
    /// Middleware for ensuring that each HTTP request includes a correlation ID.
    /// </summary>
    public class CorrelationIdMiddleware(RequestDelegate next, INoxApiLogger logger)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next;

        private readonly INoxApiLogger _logger = logger;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Invokes the middleware to check for a correlation ID in the incoming request. Disabled for development.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                string? correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
                if (string.IsNullOrEmpty(correlationId))
                {
                    _logger.LogCritical(new MissingCorrelationIdException(), NoxApiResourceService.MissingCorrelationIdMessage);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(NoxApiResourceService.CorrelationIdIsRequired);
                    return;
                }

                CorrelationContext.CorrelationId = Guid.Parse(correlationId);
                context.Response.Headers["X-Correlation-ID"] = correlationId;

                await _next(context);
            }
            finally
            {
                CorrelationContext.CorrelationId = Guid.Empty;
            }
        }

        #endregion
    }
}