using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.Logger;
using AppyNox.Services.Base.Infrastructure.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace AppyNox.Services.Base.API.Middleware
{
    /// <summary>
    /// Middleware for ensuring that each HTTP request includes a correlation ID.
    /// </summary>
    public class CorrelationIdMiddleware(RequestDelegate next, INoxApiLogger logger, IWebHostEnvironment env)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next;

        private readonly INoxApiLogger _logger = logger;

        private readonly IWebHostEnvironment _env = env;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Invokes the middleware to check for a correlation ID in the incoming request. Disabled for development.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            if (_env.IsDevelopment()) // Check if in development environment
            {
                // In development, you may choose to skip correlation ID validation
                CorrelationContext.CorrelationId = "DevelopmentCorrelationId";
                context.Response.Headers["X-Correlation-ID"] = "DevelopmentCorrelationId";
            }
            else
            {
                string? correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
                if (string.IsNullOrEmpty(correlationId))
                {
                    _logger.LogCritical(new MissingCorrelationIdException("Correlation ID is required"), "A request with no correlation ID received.");
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Correlation ID is required");
                    return;
                }

                CorrelationContext.CorrelationId = correlationId;
                context.Response.Headers["X-Correlation-ID"] = correlationId;
            }

            await _next(context);
        }

        #endregion
    }
}