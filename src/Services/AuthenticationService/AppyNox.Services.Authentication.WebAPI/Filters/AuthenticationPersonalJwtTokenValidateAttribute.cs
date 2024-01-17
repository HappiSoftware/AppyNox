using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AppyNox.Services.Authentication.WebAPI.Filters
{
    /// <summary>
    /// This filter applies AuthenticationJwtTokenValidateAttribute
    /// <para>Additionally this filter allows access to endpoints which accept "id" and token name identifier matches with the path parameter "id"</para>
    /// </summary>
    public class AuthenticationPersonalJwtTokenValidateAttribute : JwtTokenValidateAttribute
    {
        #region [ Public Methods ]

        public override void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            ICustomTokenManager manager = context.HttpContext.RequestServices.GetService(typeof(ICustomTokenManager)) as ICustomTokenManager
                ?? throw new AuthenticationApiException("Token Manager could not be initialized.");

            // Get route data (path parameter)
            var routeData = context.RouteData.Values;
            routeData.TryGetValue("id", out var routeId);

            bool isTokenValid = !string.IsNullOrWhiteSpace(token) && manager.VerifyToken(token);
            if (!isTokenValid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Determine if the user is an admin
            bool isAdmin = manager.GetIsAdmin(token);

            // Get the user info from the token
            var userInfo = manager.GetUserInfoByToken(token);

            // Admins have full access, non-admins need their routeId to match their userInfo
            if (!(isAdmin || (routeId != null && userInfo.Equals(routeId.ToString()))))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        #endregion
    }
}