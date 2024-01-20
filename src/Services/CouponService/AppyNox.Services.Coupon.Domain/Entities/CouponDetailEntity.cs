using AppyNox.Services.Base.Domain;

namespace AppyNox.Services.Coupon.Domain.Entities
{
    public class CouponDetailEntity : EntityBase
    {
        #region [ Properties ]

        public string? Detail { get; set; }

        #endregion

        #region [ Relations ]

        public virtual ICollection<CouponEntity>? Coupons { get; set; }

        #endregion
    }
}