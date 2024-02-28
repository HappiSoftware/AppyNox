using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Domain.ExceptionExtensions.Base;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class Coupon : EntityBase, IHasStronglyTypedId, IHasCode
{
    #region [ Properties ]

    public CouponId Id { get; private set; } = new CouponId(Guid.NewGuid());

    public string Description { get; private set; } = string.Empty;

    public string? Detail { get; private set; }

    public Amount Amount { get; private set; }

    #endregion

    #region [ Constructors and Factories ]

    private Coupon()
    {
    }

    private Coupon(Guid id, string code, string description, string? detail, Amount amount)
    {
        Id = new CouponId(id);
        Description = description;
        Detail = detail;
        Code = code;
        Amount = amount;
    }

    private Coupon(Guid id, string code, string description, string? detail, Amount amount, CouponDetailId couponDetailId)
        : this(id, code, description, detail, amount)
    {
        CouponDetailId = couponDetailId;
    }

    private Coupon(Guid id, string code, string description, string? detail, Amount amount, CouponDetail couponDetail)
        : this(id, code, description, detail, amount)
    {
        CouponDetail = couponDetail;
        CouponDetailId = couponDetail.Id;
    }

    public static Coupon Create(string code, string description, string? detail, Amount amount, CouponDetailId couponDetailId)
    {
        Coupon entity = new(Guid.NewGuid(), code, description, detail, amount, couponDetailId);
        return entity;
    }

    public static Coupon Create(string code, string description, string? detail, Amount amount, CouponDetail couponDetail)
    {
        Coupon entity = new(Guid.NewGuid(), code, description, detail, amount, couponDetail);
        return entity;
    }

    #endregion

    #region [ IHasCode ]

    public string Code { get; private set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public CouponDetailId CouponDetailId { get; private set; } = default!;

    public virtual CouponDetail CouponDetail { get; private set; } = null!;

    #endregion

    #region [ IEntityTypeId ]

    Guid IHasStronglyTypedId.GetTypedId => Id.Value;

    #endregion

    #region [ Update ]

    public void UpdateMinimumAmount(int minimumAmount)
    {
        Amount = Amount.UpdateMinimumAmount(minimumAmount);
    }

    public void UpdateDetail(string? detail)
    {
        Detail = detail;
    }

    #endregion
}

public sealed record CouponId(Guid Value) : IHasGuidId
{
    public Guid GetGuidValue() => Value;
}

public sealed record Amount
{
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