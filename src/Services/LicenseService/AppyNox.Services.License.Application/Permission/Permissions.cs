using System.Collections.Immutable;

namespace AppyNox.Services.License.Application.Permission;

public static class Permissions
{
    #region [ Classes ]

    public static class Licenses
    {
        #region [ Fields ]

        public const string View = "Licenses.View";

        public const string Create = "Licenses.Create";

        public const string Edit = "Licenses.Edit";

        public const string Delete = "Licenses.Delete";

        public static readonly ImmutableArray<string> Metrics =
               [View, Create, Edit, Delete];

        #endregion
    }

    public static class Products
    {
        #region [ Fields ]

        public const string View = "Products.View";

        public const string Create = "Products.Create";

        public const string Edit = "Products.Edit";

        public const string Delete = "Products.Delete";

        public static readonly ImmutableArray<string> Metrics =
               [View, Create, Edit, Delete];

        #endregion
    }

    #endregion
}