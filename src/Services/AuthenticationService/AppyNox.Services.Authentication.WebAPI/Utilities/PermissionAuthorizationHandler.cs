using AppyNox.Services.Authentication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AppyNox.Services.Authentication.WebAPI.Utilities
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        #region [ Fields ]

        private readonly IdentityDbContext _db;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        #endregion

        #region [ Public Constructors ]

        public PermissionAuthorizationHandler(IdentityDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #endregion

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
}