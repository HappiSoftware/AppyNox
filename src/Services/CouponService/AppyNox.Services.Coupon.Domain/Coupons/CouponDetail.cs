using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public class CouponDetail : EntityBase, IHasStronglyTypedId, IHasCode
{
    #region [ Properties ]

    public CouponDetailId Id { get; private set; } = new CouponDetailId(Guid.NewGuid());

    public string? Detail { get; private set; }

    #endregion

    #region [ Constructors and Factories ]

    private CouponDetail()
    {
    }

    private CouponDetail(Guid id, string code, string? detail)
    {
        Id = new CouponDetailId(id);
        Code = code;
        Detail = detail;
    }

    public static CouponDetail Create(string code, string? detail)
    {
        CouponDetail entity = new(Guid.NewGuid(), code, detail);
        return entity;
    }

    #endregion

    #region [ Relations ]

    public virtual IEnumerable<Coupon>? Coupons { get; set; }

    public virtual IEnumerable<CouponDetailTag>? CouponDetailTags { get; set; }

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