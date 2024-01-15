using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace AppyNox.Services.Base.API.Middleware
{
    public class UserIdMiddleware(RequestDelegate next, INoxApiLogger logger)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next;

        private readonly INoxApiLogger _logger = logger;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Invokes the middleware to check for a User ID in the incoming request.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                UserIdContext.UserId = userId;
            }
            else
            {
                _logger.LogWarning("UserId is empty. Could not set to UserIdContext");
            }

            await _next(context);
        }

        #endregion
    }
}