using System.ComponentModel;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.DetailLevel
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