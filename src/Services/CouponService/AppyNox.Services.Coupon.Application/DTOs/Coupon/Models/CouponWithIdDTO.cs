using AppyNox.Services.Coupon.Application.DTOs;
using AppyNox.Services.Coupon.Application.DTOs.Coupon.DetailLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Models
{
    [CouponDetailLevel(CouponDetailLevel.WithId)]
    public class CouponWithIdDTO : CouponDTO, IUpdateDTO
    {
        public Guid Id { get; set; }
        public int DetailValue { get; set; }
    }
}
