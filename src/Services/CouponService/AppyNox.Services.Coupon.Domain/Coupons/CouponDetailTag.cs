using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class CouponDetailTag : AggregateRoot, IHasStronglyTypedId
{
    #region [ Properties ]

    public CouponDetailTagId Id { get; private set; }

    public string Tag { get; private set; } = string.Empty;

    #endregion

    #region [ Constructors ]

#nullable disable

    private CouponDetailTag()
    {
    }

#nullable restore

    internal CouponDetailTag(CouponDetailTagBuilder builder)
    {
        Id = new CouponDetailTagId(Guid.NewGuid());
        Tag = builder.Tag;
        CouponDetailId = builder.CouponDetailId;
    }

    #endregion

    #region [ IEntityTypeId ]

    public Guid GetTypedId => Id.Value;

    #endregion

    #region [ Relations ]

    public CouponDetailId CouponDetailId { get; private set; }

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