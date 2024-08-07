using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Sso.Application.AsyncLocals;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;

namespace AppyNox.Services.Sso.WebAPI.Middlewares;

public class SsoContextMiddleware(RequestDelegate next, INoxApiLogger<SsoContextMiddleware> logger)
{
    #region [ Fields ]

    private readonly RequestDelegate _next = next;

    private readonly INoxApiLogger<SsoContextMiddleware> _logger = logger;

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
            #region [ IsConnectRequest ]

            string path = context.Request.Path.ToString();

            // IsConnectRequest will allow database to bypass company global filter.
            bool isConnectRequest =
                path.EndsWith("/authentication/connect/token")
                || path.EndsWith("/authentication/refresh");
            SsoContext.IsConnectRequest = isConnectRequest;

            if(isConnectRequest)
            {
                await _next(context);
                return;
            }

            #endregion

            #region [ CompanyId ]

            string? companyId = context.User.FindFirst("company")?.Value;
            if (!string.IsNullOrEmpty(companyId) && Guid.TryParse(companyId, out Guid val))
            {
                SsoContext.CompanyId = val;
            }
            else
            {
                NoxSsoApiException exception = new("CompanyId was null");
                _logger.LogCritical(exception, "CompanyId is empty. Could not set to CompanyIdContext", false);
            }

            #endregion

            #region [ IsAdmin ]

            bool isAdmin = context.User.HasClaim(c => c.Type == "role" && c.Value == "admin");
            SsoContext.IsAdmin = isAdmin;

            #endregion

            #region [ IsSuperAdmin ]

            bool isSuperAdmin = context.User.HasClaim(c => c.Type == "role" && c.Value == "superadmin");
            SsoContext.IsSuperAdmin = isSuperAdmin;

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