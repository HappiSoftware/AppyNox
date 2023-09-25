using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Authentication.WebAPI.Utilities
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public string Type { get; set; }

        public PermissionRequirement(string permission, string type)
        {
            Permission = permission;
            Type = type;
        }
    }
}
