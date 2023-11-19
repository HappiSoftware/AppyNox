using System.ComponentModel;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel
{
    #region [ Enums ]

    public enum CouponDataAccessDetailLevel
    {
        [Description("Simple")]
        Simple,

        [Description("WithAllRelations")]
        WithAllRelations
    }

    public enum CouponCreateDetailLevel
    {
        [Description("SimpleCreate")]
        SimpleCreate,

        [Description("ExtendedCreate")]
        ExtendedCreate
    }

    public enum CouponUpdateDetailLevel
    {
        [Description("SimpleUpdate")]
        SimpleUpdate,

        [Description("ExtendedUpdate")]
        ExtendedUpdate
    }

    #endregion

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CouponDetailLevelAttribute : Attribute
    {
        #region Public Constructors

        public CouponDetailLevelAttribute(CouponDataAccessDetailLevel dataAccessDetailLevel)
        {
            DataAccessDetailLevel = dataAccessDetailLevel;
        }

        public CouponDetailLevelAttribute(CouponCreateDetailLevel createDetailLevel)
        {
            CreateDetailLevel = createDetailLevel;
        }

        public CouponDetailLevelAttribute(CouponUpdateDetailLevel updateDetailLevel)
        {
            UpdateDetailLevel = updateDetailLevel;
        }

        #endregion

        #region [ Properties ]

        public CouponDataAccessDetailLevel DataAccessDetailLevel { get; }

        public CouponCreateDetailLevel CreateDetailLevel { get; }

        public CouponUpdateDetailLevel UpdateDetailLevel { get; }

        #endregion
    }
}