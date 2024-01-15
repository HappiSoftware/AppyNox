using AppyNox.Services.Base.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AppyNox.Services.Base.API.Extensions
{
    public static class WebApplicationExtensions
    {
        #region [ Public Methods ]

        public static void AddAsyncLocalContextMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<UserIdMiddleware>();
        }

        #endregion
    }
}