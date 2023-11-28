using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

            // Set the correlation ID in the request context
            context.Items["CorrelationId"] = correlationId;

            // Continue processing the request
            await _next(context);
        }

        #endregion
    }
}