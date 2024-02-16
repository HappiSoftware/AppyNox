using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.DetailLevel;

public enum CouponDetailTagDataAccessDetailLevel
{
    [Display(Name = "Simple")]
    Simple,

    [Display(Name = "WithAllRelations")]
    WithAllRelations
}

public enum CouponDetailTagCreateDetailLevel
{
    [Display(Name = "Simple")]
    Simple,

    [Display(Name = "Extended")]
    Extended
}

public enum CouponDetailTagUpdateDetailLevel
{
    [Display(Name = "Simple")]
    Simple,

    [Display(Name = "Extended")]
    Extended
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class CouponDetailTagDetailLevelAttribute : Attribute
{
    #region [ Public Constructors ]

    public CouponDetailTagDetailLevelAttribute(CouponDetailTagDataAccessDetailLevel dataAccessDetailLevel)
    {
        DataAccessDetailLevel = dataAccessDetailLevel;
    }

    public CouponDetailTagDetailLevelAttribute(CouponDetailTagCreateDetailLevel createDetailLevel)
    {
        CreateDetailLevel = createDetailLevel;
    }

    public CouponDetailTagDetailLevelAttribute(CouponDetailTagUpdateDetailLevel updateDetailLevel)
    {
        UpdateDetailLevel = updateDetailLevel;
    }

    #endregion

    #region [ Properties ]

    public CouponDetailTagDataAccessDetailLevel DataAccessDetailLevel { get; }

    public CouponDetailTagCreateDetailLevel CreateDetailLevel { get; }

    public CouponDetailTagUpdateDetailLevel UpdateDetailLevel { get; }

    #endregion
}