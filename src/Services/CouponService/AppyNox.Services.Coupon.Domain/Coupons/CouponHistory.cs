﻿using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class CouponHistory : AggregateMember, IHasStronglyTypedId
{
    #region [ Properties ]

    public CouponHistoryId Id { get; private set; }

    public int MinimumAmount { get; private set; }

    public DateTime Date { get; private set; }

    #endregion

    #region [ Relations ]

    public virtual CouponId CouponId { get; private set; }

    #endregion

    #region [ IHasStronglyTypedId ]

    public Guid GetTypedId() => Id.Value;

    #endregion

    #region [ Constructors ]

#nullable disable

    /// <summary>
    /// For ef core migration creating. Do not use this constructor in actual implementations
    /// </summary>
    protected CouponHistory()
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

public record CouponHistoryId(Guid Value) : NoxId(Value)
{
}

#endregion