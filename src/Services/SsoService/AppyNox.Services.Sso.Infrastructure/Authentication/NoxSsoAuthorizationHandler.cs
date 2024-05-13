using AppyNox.Services.Base.Infrastructure.Authentication;
using AppyNox.Services.Sso.Infrastructure.Exceptions;
using AppyNox.Services.Sso.Infrastructure.Exceptions.Base;
using AppyNox.Services.Sso.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Sso.Infrastructure.Authentication;

public class NoxSsoAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
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
                throw new NoxAuthorizationException(NoxSsoInfrastructureResourceService.UnauthorizedAccess, (int)NoxSsoInfrastructureExceptionCode.AuthorizationFailed);
            }
        }
        else
        {
            throw new NoxAuthorizationException(NoxSsoInfrastructureResourceService.InvalidToken, (int)NoxSsoInfrastructureExceptionCode.AuthorizationInvalidToken);
        }

        return Task.CompletedTask;
    }

    #endregion
}