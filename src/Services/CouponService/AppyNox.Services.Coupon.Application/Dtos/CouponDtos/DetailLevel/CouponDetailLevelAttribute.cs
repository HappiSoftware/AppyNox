using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel
{
    #region [ Enums ]

    public enum CouponDataAccessDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "WithAllRelations")]
        WithAllRelations
    }

    public enum CouponCreateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Bulk")]
        Bulk,

        [Display(Name = "Extended")]
        Extended
    }

    public enum CouponUpdateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Extended")]
        Extended
    }

    #endregion

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CouponDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

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