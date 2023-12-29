using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.Logger;
using AppyNox.Services.Base.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AppyNox.Services.Base.API.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next, INoxApiLogger logger)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next;

        private readonly INoxApiLogger _logger = logger;

        #endregion

        #region [ Public Methods ]

        public async Task Invoke(HttpContext context)
        {
            string? correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
            if (string.IsNullOrEmpty(correlationId))
            {
                // If the Correlation ID is missing, reject the request
                _logger.LogCritical(new MissingCorrelationIdException("Correlation ID is required"), "A request with no correlation ID received.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Correlation ID is required");
                return;
            }

            CorrelationContext.CorrelationId = correlationId; // Set the correlation ID
            context.Response.Headers["X-Correlation-ID"] = correlationId; // Optionally echo back the correlation ID
            await _next(context);
        }

        #endregion
    }
}