using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Authentication.WebAPI.Utilities
{
    /// <summary>
    /// Specifies a requirement for having a particular permission.
    /// </summary>
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the PermissionRequirement class with the specified permission and type.
        /// </summary>
        /// <param name="permission">The permission required.</param>
        /// <param name="type">The type of the requirement.</param>
        public PermissionRequirement(string permission, string type)
        {
            Permission = permission;
            Type = type;
        }

        #endregion

        #region [ Properties ]

        public string Permission { get; private set; }

        public string Type { get; set; }

        #endregion
    }
}