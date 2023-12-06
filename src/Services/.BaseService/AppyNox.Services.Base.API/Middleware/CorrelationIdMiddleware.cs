using Microsoft.AspNetCore.Http;

namespace AppyNox.Services.Base.API.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        #region Fields

        private readonly RequestDelegate _next = next;

        #endregion

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            // Generate or retrieve the correlation ID from the incoming request
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            if(string.IsNullOrEmpty(correlationId))
            {
                // Set the correlation ID in the request headers
                context.Request.Headers["X-Correlation-ID"] = Guid.NewGuid().ToString();
            }

            // Continue processing the request
            await _next(context);
        }

        #endregion
    }
}