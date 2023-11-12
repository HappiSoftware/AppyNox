using System.ComponentModel;

namespace AppyNox.Services.Coupon.Application.Dtos.Coupon.DetailLevel
{
    public enum CouponDetailLevel
    {
        [Description("Basic")]
        Basic,

        [Description("WithId")]
        WithId,

        [Description("WithAllProperties")]
        WithAllProperties
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CouponDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public CouponDetailLevelAttribute(CouponDetailLevel level)
        {
            DetailLevel = level;
        }

        #endregion

        #region [ Properties ]

        public CouponDetailLevel DetailLevel { get; }

        #endregion
    }
}