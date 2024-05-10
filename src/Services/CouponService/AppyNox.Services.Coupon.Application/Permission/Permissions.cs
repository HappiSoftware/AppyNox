using System.Collections.Immutable;

namespace AppyNox.Services.Coupon.Application.Permission;

public static class Permissions
{
    #region [ Classes ]

    public static class Coupons
    {
        #region [ Fields ]

        public const string View = "Coupons.View";

        public const string Create = "Coupons.Create";

        public const string Edit = "Coupons.Edit";

        public const string Delete = "Coupons.Delete";

        public static readonly ImmutableArray<string> Metrics =
               [View, Create, Edit, Delete];

        #endregion
    }
    public static class CouponsAdmin
    {
        #region [ Fields ]

        public const string View = "Coupons.View";

        public static readonly ImmutableArray<string> Metrics =
               [View];

        #endregion
    }

    #endregion
}