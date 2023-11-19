using System.ComponentModel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel
{
    public enum CouponDetailDetailLevel
    {
        [Description("Simple")]
        Simple,

        [Description("WithAllRelations")]
        WithAllRelations
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CouponDetailDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public CouponDetailDetailLevelAttribute(CouponDetailDetailLevel level)
        {
            DetailLevel = level;
        }

        #endregion

        #region [ Properties ]

        public CouponDetailDetailLevel DetailLevel { get; }

        #endregion
    }
}