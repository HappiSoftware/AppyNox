using AppyNox.Services.Coupon.Application.DTOs.Coupon.DetailLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Models
{
    [CouponDetailLevel(CouponDetailLevel.Basic)]
    public class CouponDTO : BaseDTO
    {
        public string Code { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
