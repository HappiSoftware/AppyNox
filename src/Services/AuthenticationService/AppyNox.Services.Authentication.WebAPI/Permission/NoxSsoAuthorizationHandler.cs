using AppyNox.Services.Base.API.ExceptionExtensions;
using AppyNox.Services.Base.API.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Authentication.WebAPI.Permission
{
    public class NoxSsoAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        #region [ Protected Methods ]

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "superadmin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(c => c.Type == "nameid"))
            {
                if (context.User.HasClaim(c => c.Value == requirement.Permission))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    throw new NoxAuthorizationException();
                }
            }
            else
            {
                throw new NoxAuthorizationException();
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}