using AppyNox.Services.Base.API.Exceptions;
using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.API.Localization;
using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Base.API.Permissions;

public class NoxJwtAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
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