using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace AppyNox.Services.Authentication.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class JwtTokenValidateAttribute : Attribute, IAuthorizationFilter
    {
        #region [ Public Methods ]

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrWhiteSpace(token) || (context.HttpContext.RequestServices.GetService(typeof(ICustomTokenManager)) as ICustomTokenManager)?.VerifyToken(token) == false)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        #endregion
    }
}