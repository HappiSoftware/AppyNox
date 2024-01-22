using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.API.Middleware.Options;
using Microsoft.AspNetCore.Builder;

namespace AppyNox.Services.Base.API.Extensions
{
    public static class WebApplicationExtensions
    {
        #region [ Public Methods ]

        public static IApplicationBuilder UseNoxResponseWrapper(this IApplicationBuilder builder, NoxResponseWrapperOptions options)
        {
            return builder.UseMiddleware<NoxResponseWrapperMiddleware>(options);
        }

        public static IApplicationBuilder UseCorrelationContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }

        public static IApplicationBuilder UseUserIdContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdMiddleware>();
        }

        #endregion
    }
}