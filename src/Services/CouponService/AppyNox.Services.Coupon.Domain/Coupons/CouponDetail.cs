using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class CouponDetail : EntityBase, IHasStronglyTypedId, IHasCode
{
    #region [ Properties ]

    public CouponDetailId Id { get; private set; }

    public string? Detail { get; private set; }

    #endregion

#nullable disable

    #region [ Constructors ]

    private CouponDetail()
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

    public ICollection<CouponDetailTag>? CouponDetailTags { get; set; }

    #endregion

    #region [ IHasCode ]

    public string Code { get; private set; } = string.Empty;

    #endregion

    #region [ IEntityTypeId ]

    Guid IHasStronglyTypedId.GetTypedId => Id.Value;

    #endregion
}

public sealed record CouponDetailId : IHasGuidId
{
    public Guid Value { get; private set; }
    Guid IHasGuidId.GetGuidValue()
    {
        return Value;
    }
    private CouponDetailId()
    {
    }
    public CouponDetailId(Guid value)
    {
        Value = value;
    }
}