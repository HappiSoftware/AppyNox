using AppyNox.Services.Base.API.Permissions;
using AppyNox.Services.Sso.WebAPI.Exceptions;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;
using AppyNox.Services.Sso.WebAPI.Localization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppyNox.Services.Sso.WebAPI.Permission;

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
                throw new NoxAuthorizationException(NoxSsoApiResourceService.UnauthorizedAccess, (int)NoxSsoApiExceptionCode.AuthorizationFailed);
            }
        }
        else
        {
            throw new NoxAuthorizationException(NoxSsoApiResourceService.InvalidToken, (int)NoxSsoApiExceptionCode.AuthorizationInvalidToken);
        }

        return Task.CompletedTask;
    }

    #endregion
}