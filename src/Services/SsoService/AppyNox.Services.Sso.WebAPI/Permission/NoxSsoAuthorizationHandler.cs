using AppyNox.Services.Base.API.Permissions;
using AppyNox.Services.Sso.WebAPI.Exceptions;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;
using AppyNox.Services.Sso.WebAPI.Localization;
using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Sso.WebAPI.Permission;

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