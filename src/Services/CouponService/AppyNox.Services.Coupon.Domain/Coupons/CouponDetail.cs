using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class CouponDetail : AggregateMember, IHasStronglyTypedId, IHasCode
{
    #region [ Properties ]

    public CouponDetailId Id { get; private set; }

    public string? Detail { get; private set; }

    #endregion

    #region [ Constructors ]

#nullable disable

    protected CouponDetail()
    {
    }

#nullable restore

    internal CouponDetail(CouponDetailBuilder builder)
    {
        Id = new CouponDetailId(Guid.NewGuid());
        Code = builder.Code;
        Detail = builder.Detail;
    }

    #endregion

    #region [ Relations ]

    public virtual ICollection<CouponDetailTag>? CouponDetailTags { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; private set; } = string.Empty;

    #endregion

    #region [ IHasStronglyTypedId ]

    public Guid GetTypedId() => Id.Value;

    #endregion
}

public record CouponDetailId : IHasGuidId, IValueObject
{
    protected CouponDetailId() { }
    public CouponDetailId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; private set; }
    public Guid GetGuidValue() => Value;
}