using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class CouponDetailTag : EntityBase, IHasStronglyTypedId
{
    #region [ Properties ]

    public CouponDetailTagId Id { get; private set; } = new CouponDetailTagId(Guid.NewGuid());

    public string Tag { get; private set; } = string.Empty;

    #endregion

    #region [ Constructors and Factories ]

    private CouponDetailTag()
    {
    }

    private CouponDetailTag(Guid id, string tag, CouponDetailId couponDetailId)
    {
        Id = new CouponDetailTagId(id);
        Tag = tag;
        CouponDetailId = couponDetailId;
    }

    public static CouponDetailTag Create(string tag, CouponDetailId couponDetailId)
    {
        CouponDetailTag entity = new(Guid.NewGuid(), tag, couponDetailId);
        return entity;
    }

    #endregion

    #region [ IEntityTypeId ]

    public Guid GetTypedId => Id.Value;

    #endregion

    #region [ Relations ]

    public CouponDetailId CouponDetailId { get; private set; }

    public virtual CouponDetail CouponDetail { get; private set; } = null!;

    #endregion
}

public sealed record CouponDetailTagId : IHasGuidId
{
    public Guid Value { get; private set; }
    Guid IHasGuidId.GetGuidValue()
    {
        return Value;
    }
    private CouponDetailTagId()
    {
    }
    public CouponDetailTagId(Guid value)
    {
        Value = value;
    }
}