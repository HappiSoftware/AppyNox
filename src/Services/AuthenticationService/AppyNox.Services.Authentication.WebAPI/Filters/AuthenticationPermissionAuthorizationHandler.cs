using AppyNox.Services.Base.API.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.WebAPI.Filters
{
    public class AuthenticationPermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        #region [ Protected Methods ]

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "superadmin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                if (context.User.HasClaim(c => c.Value == requirement.Permission))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}