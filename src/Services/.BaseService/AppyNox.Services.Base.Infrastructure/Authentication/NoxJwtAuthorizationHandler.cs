using AppyNox.Services.Base.Infrastructure.Exceptions;
using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Base.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Base.Infrastructure.Authentication;

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
                throw new NoxAuthorizationException(NoxInfrastructureResourceService.UnauthorizedAccess, (int)NoxInfrastructureExceptionCode.AuthorizationFailed);
            }
        }
        else
        {
            throw new NoxAuthorizationException(NoxInfrastructureResourceService.InvalidToken, (int)NoxInfrastructureExceptionCode.AuthorizationInvalidToken);
        }

        return Task.CompletedTask;
    }

    #endregion
}