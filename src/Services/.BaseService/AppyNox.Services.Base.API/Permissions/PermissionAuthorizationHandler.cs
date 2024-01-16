using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppyNox.Services.Base.API.Permissions;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    #region [ Protected Methods ]

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
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