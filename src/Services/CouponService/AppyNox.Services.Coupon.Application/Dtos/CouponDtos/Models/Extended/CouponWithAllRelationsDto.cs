using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended
{
    [CouponDetailLevel(CouponDataAccessDetailLevel.WithAllRelations)]
    public class CouponWithAllRelationsDto : CouponExtendedCreateDto
    {
        #region [ Relations ]

        public CouponIdDto Id { get; set; }

        public virtual CouponDetailSimpleDto CouponDetailEntity { get; set; } = null!;

        #endregion
    }
}