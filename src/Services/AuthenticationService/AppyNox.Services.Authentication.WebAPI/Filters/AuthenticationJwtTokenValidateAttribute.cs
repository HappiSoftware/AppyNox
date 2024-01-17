﻿using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Base.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace AppyNox.Services.Authentication.WebAPI.Filters
{
    /// <summary>
    /// This filter applies that the requests must have valid token and requesting user must be admin.
    /// </summary>
    public class AuthenticationJwtTokenValidateAttribute : JwtTokenValidateAttribute
    {
        #region [ Public Methods ]

        public override void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            ICustomTokenManager manager = context.HttpContext.RequestServices.GetService(typeof(ICustomTokenManager)) as ICustomTokenManager
                ?? throw new AuthenticationApiException("Token Manager could not be initialized.");

            // Determine if the user is an admin
            bool isAdmin = manager.GetIsAdmin(token);

            bool isTokenValid = !string.IsNullOrWhiteSpace(token) && manager.VerifyToken(token);
            if (!isTokenValid || !isAdmin)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        #endregion
    }
}