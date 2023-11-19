using AppyNox.Services.Coupon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Domain.Entities
{
    public class CouponDetailEntity : IEntityWithGuid
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string? Detail { get; set; }

        #endregion

        #region [ Relations ]

        public virtual ICollection<CouponEntity>? Coupons { get; set; }

        #endregion
    }
}