using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Entities;

public class CouponDetailTag : EntityBase, IEntityTypeId
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string Tag { get; set; } = string.Empty;

    #endregion

    #region [ IEntityTypeId ]

    public Guid GetTypedId => Id;

    #endregion

    #region [ Relations ]

    public Guid CouponDetailEntityId { get; set; }

    public virtual CouponDetailEntity CouponDetailEntity { get; set; } = null!;

    #endregion
}