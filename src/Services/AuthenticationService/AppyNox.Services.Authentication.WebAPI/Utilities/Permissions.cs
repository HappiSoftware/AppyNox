namespace AppyNox.Services.Authentication.WebAPI.Utilities
{
    public static class Permissions
    {
        public static class Roles
        {
            public const string View = "Roles.View";
            public const string Create = "Roles.Create";
            public const string Edit = "Roles.Edit";
            public const string Delete = "Roles.Delete";
            public const string AssignPermission = "Roles.AssignPermission";
            public const string WithdrawPermission = "Roles.WithdrawPermission";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete,AssignPermission,WithdrawPermission
               });
        }

        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Edit = "Users.Edit";
            public const string Delete = "Users.Delete";
            public static readonly List<string> _metrics =
               new List<string>(new[]
               {
                    View,Create,Edit,Delete
               });
        }
    }
}
