using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class CouponDetailTag : AggregateMember, IHasStronglyTypedId
{
    #region [ Properties ]

    public CouponDetailTagId Id { get; private set; }

    public string Tag { get; private set; } = string.Empty;

    #endregion

    #region [ Constructors ]

#nullable disable

    protected CouponDetailTag()
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

    #region [ IHasStronglyTypedId ]

    public Guid GetTypedId() => Id.Value;

    #endregion

    #region [ Relations ]

    public virtual CouponDetailId CouponDetailId { get; private set; }

    #endregion
}

public record CouponDetailTagId : NoxId
{
    protected CouponDetailTagId() { }
    public CouponDetailTagId(Guid value)
    {
        Value = value;
    }
}