﻿using AppyNox.Services.Authentication.Infrastructure.AsyncLocals;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.WebAPI.Middlewares
{
    public class AuthenticationContextMiddleware(RequestDelegate next, INoxApiLogger logger)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next;

        private readonly INoxApiLogger _logger = logger;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Invokes the middleware to check for a Company ID in the incoming request.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                #region [ CompanyId ]

                string? companyId = context.User.FindFirst("company")?.Value;
                if (!string.IsNullOrEmpty(companyId) && Guid.TryParse(companyId, out Guid val))
                {
                    AuthenticationContext.CompanyId = val;
                }
                else
                {
                    _logger.LogWarning("CompanyId is empty. Could not set to CompanyIdContext");
                }

                #endregion

                #region [ IsAdmin ]

                string? isAdmin = context.User.FindFirst("admin")?.Value;
                if (!string.IsNullOrEmpty(isAdmin))
                {
                    AuthenticationContext.IsAdmin = true;
                }
                else
                {
                    AuthenticationContext.IsAdmin = false;
                }

                #endregion

                #region [ IsSuperAdmin ]

                string? isSuperAdmin = context.User.FindFirst("superadmin")?.Value;
                if (!string.IsNullOrEmpty(isSuperAdmin))
                {
                    AuthenticationContext.IsSuperAdmin = true;
                }
                else
                {
                    AuthenticationContext.IsSuperAdmin = false;
                }

                #endregion

                #region [ IsConnectRequest ]

                bool isConnectRequest = context.Request.Path.Equals("/api/authentication/connect/token");
                AuthenticationContext.IsConnectRequest = isConnectRequest;
                bool allowContinue = isConnectRequest || context.Request.Path.Equals("/api/health");

                #endregion

                #region [ UserId ]

                string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid userIdGuid))
                {
                    AuthenticationContext.UserId = userIdGuid;
                }
                else if (!allowContinue)
                {
                    _logger.LogWarning("UserId is empty. Could not set to UserIdContext");
                    throw new NoxAuthenticationApiException("Jwt User Id is empty or invalid.");
                }

                #endregion

                await _next(context);
            }
            finally
            {
                // Clear the context values at the end of each request
                AuthenticationContext.CompanyId = Guid.Empty;
                AuthenticationContext.IsAdmin = false;
                AuthenticationContext.IsSuperAdmin = false;
                AuthenticationContext.UserId = Guid.Empty;
                AuthenticationContext.IsConnectRequest = false;
            }
        }

        #endregion
    }
}