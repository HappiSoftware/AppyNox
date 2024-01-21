using AppyNox.Services.Base.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace AppyNox.Services.Base.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    [Obsolete("JwtTokenValidateAttribute is deprecated. Use NoxJwtAuthenticationHandler instead. This class will be removed in v1.0.5")]
    public class JwtTokenValidateAttribute : Attribute, IAsyncAuthorizationFilter
    {
        #region [ Public Methods ]

        async Task IAsyncAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrWhiteSpace(token) || await (context.HttpContext.RequestServices.GetService(typeof(INoxTokenManager)) as INoxTokenManager)?.VerifyToken(token)! == false)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        #endregion
    }
}