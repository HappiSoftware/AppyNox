using System.Collections.Immutable;

namespace AppyNox.Services.Sso.WebAPI.Permission
{
    /// <summary>
    /// Defines static permission strings for role and user operations.
    /// </summary>
    public static class Permissions
    {
        #region [ Classes ]

        public static class Roles
        {
            #region [ Fields ]

            public const string View = "Roles.View";

            public const string Create = "Roles.Create";

            public const string Edit = "Roles.Edit";

            public const string Delete = "Roles.Delete";

            public const string AssignPermission = "Roles.AssignPermission";

            public const string WithdrawPermission = "Roles.WithdrawPermission";

            public static readonly ImmutableArray<string> Metrics =
               [View, Create, Edit, Delete, AssignPermission, WithdrawPermission];

            #endregion
        }

        public static class Users
        {
            #region [ Fields ]

            public const string View = "Users.View";

            public const string Create = "Users.Create";

            public const string Edit = "Users.Edit";

            public const string Delete = "Users.Delete";

            public static readonly ImmutableArray<string> Metrics =
               [View, Create, Edit, Delete];

            #endregion
        }

        #endregion
    }
}