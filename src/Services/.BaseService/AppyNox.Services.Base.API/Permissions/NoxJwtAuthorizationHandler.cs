using AppyNox.Services.Base.API.ExceptionExtensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppyNox.Services.Base.API.Permissions;

public class NoxJwtAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    #region [ Protected Methods ]

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
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