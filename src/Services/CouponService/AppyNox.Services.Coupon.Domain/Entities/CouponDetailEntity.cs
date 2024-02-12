using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Entities
{
    public class CouponDetailEntity : EntityBase, IEntityTypeId, IHasCode
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string? Detail { get; set; }

        #endregion

        #region [ Relations ]

        public virtual ICollection<CouponEntity>? Coupons { get; set; }

        #endregion

        #region [ IHasCode ]

        public string Code { get; set; } = string.Empty;

        #endregion

        #region [ IEntityTypeId ]

        Guid IEntityTypeId.GetTypedId => Id;

        #endregion
    }
}