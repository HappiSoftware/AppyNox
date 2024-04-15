using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AppyNox.Services.Coupon.Domain.Exceptions.Base;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class Coupon : AggregateRoot, IHasStronglyTypedId, IHasCode
{
    #region [ Properties ]

    public CouponId Id { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public string? Detail { get; private set; }

    public Amount Amount { get; private set; }

    #endregion

    #region [ Constructors ]

    /// <summary>
    /// For ef core migration creating. Do not use this constructor in actual implementations
    /// </summary>
#nullable disable

    protected Coupon()
    {
    }

#nullable restore

    internal Coupon(CouponBuilder builder)
    {
        Id = new CouponId(Guid.NewGuid());
        Code = builder.Code;
        Description = builder.Description;
        Detail = builder.Detail;
        Amount = builder.Amount;
        CouponDetailId = builder.CouponDetailId;
        CouponDetail = builder.BulkCreate ? builder.CouponDetail : default!;
    }

    #endregion

    #region [ IHasCode ]

    public string Code { get; private set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public virtual CouponDetailId CouponDetailId { get; private set; }

    public virtual CouponDetail CouponDetail { get; private set; }

    public virtual ICollection<CouponHistory>? Histories { get; private set; }

    #endregion

    #region [ IHasStronglyTypedId ]

    public Guid GetTypedId() => Id.Value;

    #endregion

    #region [ Update ]

    public void UpdateMinimumAmount(int minimumAmount)
    {
        int oldAmount = Amount.MinAmount;
        Amount = Amount.UpdateMinimumAmount(minimumAmount);
        Raise(new CouponUpdatedDomainEvent(Id, oldAmount));
    }

    public void UpdateDetail(string? detail)
    {
        Detail = detail;
    }

    #endregion
}

public record CouponId : IHasGuidId, IValueObject
{
    protected CouponId() { }
    public CouponId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; private set; }
    public Guid GetGuidValue() => Value;
}

public record Amount : IValueObject
{
    protected Amount() { }
    public double DiscountAmount { get; private set; }

    public int MinAmount { get; private set; }

    public Amount(double discountAmount, int minAmount)
    {
        if (discountAmount <= 0)
        {
            throw new NoxCouponDomainException("Discount amount can not be lower or equal than 0", (int)NoxCouponDomainExceptionCode.AmountValidation);
        }
        if (minAmount <= 0)
        {
            throw new NoxCouponDomainException("Minimum amount can not be lower or equal than 0", (int)NoxCouponDomainExceptionCode.AmountValidation);
        }
        if (discountAmount > minAmount) // Example business logic validation which is not suitable on fluentvalidation
        {
            throw new NoxCouponDomainException("Minimum amount can not be higher than Discount amount", (int)NoxCouponDomainExceptionCode.AmountValidation);
        }
        DiscountAmount = discountAmount;
        MinAmount = minAmount;
    }

    internal Amount UpdateMinimumAmount(int minimumAmount)
    {
        if (minimumAmount <= 0)
        {
            throw new NoxCouponDomainException("Minimum amount can not be lower or equal than 0", (int)NoxCouponDomainExceptionCode.AmountValidation);
        }
        if (DiscountAmount > minimumAmount) // Example business logic validation which is not suitable on fluentvalidation
        {
            throw new NoxCouponDomainException("Minimum amount can not be higher than Discount amount", (int)NoxCouponDomainExceptionCode.AmountValidation);
        }

        return this with { MinAmount = minimumAmount };
    }
}