using AppyNox.Services.Sso.Infrastructure.AsyncLocals;
using AppyNox.Services.Sso.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using System.Security.Claims;

namespace AppyNox.Services.Sso.WebAPI.Middlewares
{
    public class SsoContextMiddleware(RequestDelegate next, INoxApiLogger logger)
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
                    SsoContext.CompanyId = val;
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
                    SsoContext.IsAdmin = true;
                }
                else
                {
                    SsoContext.IsAdmin = false;
                }

                #endregion

                #region [ IsSuperAdmin ]

                string? isSuperAdmin = context.User.FindFirst("superadmin")?.Value;
                if (!string.IsNullOrEmpty(isSuperAdmin))
                {
                    SsoContext.IsSuperAdmin = true;
                }
                else
                {
                    SsoContext.IsSuperAdmin = false;
                }

                #endregion

                #region [ IsConnectRequest ]

                string path = context.Request.Path.ToString();

                // IsConnectRequest will allow database to bypass company global filter.
                bool isConnectRequest =
                    path.EndsWith("/authentication/connect/token")
                    || path.EndsWith("/authentication/refresh");
                SsoContext.IsConnectRequest = isConnectRequest;

                #endregion

                await _next(context);
            }
            finally
            {
                // Clear the context values at the end of each request
                SsoContext.CompanyId = Guid.Empty;
                SsoContext.IsAdmin = false;
                SsoContext.IsSuperAdmin = false;
                SsoContext.IsConnectRequest = false;
            }
        }

        #endregion
    }
}
