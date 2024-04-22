using AppyNox.Services.Base.API.Exceptions;
using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.API.Localization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppyNox.Services.Base.API.Permissions;

public class NoxJwtAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    #region [ Protected Methods ]

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Claims.Any(c => c.Type == "role" && c.Value == "superadmin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.HasClaim(c => c.Type == "nameid"))
        {
            if (context.User.HasClaim(c => c.Type == requirement.Type && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            else
            {
                throw new NoxAuthorizationException(NoxApiResourceService.UnauthorizedAccess, (int)NoxApiExceptionCode.AuthorizationFailed);
            }
        }
        else
        {
            throw new NoxAuthorizationException(NoxApiResourceService.InvalidToken, (int)NoxApiExceptionCode.AuthorizationInvalidToken);
        }

        return Task.CompletedTask;
    }

    #endregion
}