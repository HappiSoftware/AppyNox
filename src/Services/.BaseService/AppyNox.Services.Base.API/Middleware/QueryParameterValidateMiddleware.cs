using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AppyNox.Services.Base.API.Middleware
{
    /// <summary>
    /// Middleware for validating query parameters in the request.
    /// </summary>
    public class QueryParameterValidateMiddleware(RequestDelegate next)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

        #endregion

        #region [ Public Methods ]

        // <summary>
        /// Invokes the middleware to validate query parameters in the incoming request.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var accessValue = context.Request.Query["Access"].ToString();

            if (!string.IsNullOrEmpty(accessValue) && !Enum.TryParse<DtoLevelMappingTypes>(accessValue, true, out var result))
            {
                string message = NoxApiResourceService.InvalidAccessModifier.Format(accessValue);
                throw new NoxApiException(message, (int)HttpStatusCode.BadRequest);
            }
            await _next(context);
        }

        #endregion
    }
}