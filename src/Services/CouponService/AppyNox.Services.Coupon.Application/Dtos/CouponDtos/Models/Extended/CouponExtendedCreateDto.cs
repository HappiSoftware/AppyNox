using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended
{
    [CouponDetailLevel(CouponCreateDetailLevel.Extended)]
    public class CouponExtendedCreateDto : CouponSimpleCreateDto
    {
        #region [ Properties ]

        public string? Detail { get; set; }

        #endregion
    }
}