using AppyNox.Services.Base.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Domain.Entities
{
    public class CouponEntity : IEntityWithGuid
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

        public int DetailValue { get; set; }

        #endregion

        #region [ Relations ]

        public Guid CouponDetailEntityId { get; set; }

        public virtual CouponDetailEntity CouponDetailEntity { get; set; } = null!;

        #endregion
    }
}