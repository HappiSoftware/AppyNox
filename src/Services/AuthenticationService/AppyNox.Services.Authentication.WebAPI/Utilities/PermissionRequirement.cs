﻿using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Authentication.WebAPI.Utilities
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        #region [ Public Constructors ]

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