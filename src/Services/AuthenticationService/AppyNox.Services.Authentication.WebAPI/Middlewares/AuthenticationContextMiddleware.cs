using AppyNox.Services.Authentication.Infrastructure.AsyncLocals;
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

                string path = context.Request.Path.ToString();

                // IsConnectRequest will allow database to bypass company global filter.
                bool isConnectRequest = path.EndsWith("/authentication/connect/token") || path.EndsWith("/authentication/refresh");
                AuthenticationContext.IsConnectRequest = isConnectRequest;

                #endregion

                await _next(context);
            }
            finally
            {
                // Clear the context values at the end of each request
                AuthenticationContext.CompanyId = Guid.Empty;
                AuthenticationContext.IsAdmin = false;
                AuthenticationContext.IsSuperAdmin = false;
                AuthenticationContext.IsConnectRequest = false;
            }
        }

        #endregion
    }
}