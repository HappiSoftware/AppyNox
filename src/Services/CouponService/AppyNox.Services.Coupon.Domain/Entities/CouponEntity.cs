using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Entities
{
    public class CouponEntity : EntityBase, IEntityTypeId, IAuditableData, IHasCode
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public string? Detail { get; set; }

        #endregion

        #region [ IAuditableData ]

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdateDate { get; set; }

        #endregion

        #region [ IHasCode ]

        public string Code { get; set; } = string.Empty;

        #endregion

        #region [ Relations ]

        public Guid CouponDetailEntityId { get; set; }

        public virtual CouponDetailEntity CouponDetailEntity { get; set; } = null!;

        #endregion

        #region [ IEntityTypeId ]

        Guid IEntityTypeId.GetTypedId => Id;

        #endregion
    }
}