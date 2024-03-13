using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public sealed class CouponHistory : AggregateRoot, IHasStronglyTypedId
{
    #region [ Properties ]

    public CouponHistoryId Id { get; private set; }

    public int MinimumAmount { get; private set; }

    public DateTime Date { get; private set; }

    #endregion

    #region [ Relations ]

    public CouponId CouponId { get; private set; }

    #endregion

    #region [ IHasStronglyTypedId ]

    public Guid GetTypedId => Id.Value;

    #endregion

    #region [ Constructors ]

#nullable disable

    /// <summary>
    /// For ef core migration creating. Do not use this constructor in actual implementations
    /// </summary>
    private CouponHistory()
    {
    }

#nullable enable

    public CouponHistory(CouponId couponId, int minimumAmount)
    {
        Id = new CouponHistoryId(Guid.NewGuid());
        CouponId = couponId;
        MinimumAmount = minimumAmount;
        Date = DateTime.UtcNow;
    }

    #endregion
}

#region [ Value Objects ]

public sealed record CouponHistoryId(Guid Value) : IHasGuidId
{
    public Guid GetGuidValue() => Value;
}

#endregion