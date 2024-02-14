using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.API.Middleware.Options;
using Microsoft.AspNetCore.Builder;

namespace AppyNox.Services.Base.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        #region [ Public Methods ]

        public static IApplicationBuilder UseNoxResponseWrapper(this IApplicationBuilder builder, NoxResponseWrapperOptions options)
        {
            return builder.UseMiddleware<NoxResponseWrapperMiddleware>(options);
        }

        public static IApplicationBuilder UseNoxContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NoxContextMiddleware>();
        }

        #endregion
    }
}