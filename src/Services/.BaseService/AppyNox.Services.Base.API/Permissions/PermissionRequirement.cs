﻿using Microsoft.AspNetCore.Authorization;

namespace AppyNox.Services.Base.API.Permissions;

public class PermissionRequirement(string permission, string type) : IAuthorizationRequirement
{
    #region [ Properties ]

    public string Permission { get; private set; } = permission;

    public string Type { get; set; } = type;

    #endregion
}