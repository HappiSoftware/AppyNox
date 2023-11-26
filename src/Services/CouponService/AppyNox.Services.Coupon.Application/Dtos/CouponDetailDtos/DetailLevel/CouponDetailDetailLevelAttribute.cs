using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel
{
    public enum CouponDetailDataAccessDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "WithAllRelations")]
        WithAllRelations
    }

    public enum CouponDetailCreateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Extended")]
        Extended
    }

    public enum CouponDetailUpdateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Extended")]
        Extended
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CouponDetailDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public CouponDetailDetailLevelAttribute(CouponDetailDataAccessDetailLevel dataAccessDetailLevel)
        {
            DataAccessDetailLevel = dataAccessDetailLevel;
        }

        public CouponDetailDetailLevelAttribute(CouponDetailCreateDetailLevel createDetailLevel)
        {
            CreateDetailLevel = createDetailLevel;
        }

        public CouponDetailDetailLevelAttribute(CouponDetailUpdateDetailLevel updateDetailLevel)
        {
            UpdateDetailLevel = updateDetailLevel;
        }

        #endregion

        #region [ Properties ]

        public CouponDetailDataAccessDetailLevel DataAccessDetailLevel { get; }

        public CouponDetailCreateDetailLevel CreateDetailLevel { get; }

        public CouponDetailUpdateDetailLevel UpdateDetailLevel { get; }

        #endregion
    }
}