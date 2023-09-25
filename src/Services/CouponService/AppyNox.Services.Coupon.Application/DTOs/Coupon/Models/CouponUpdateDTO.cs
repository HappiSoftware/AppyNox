using AppyNox.Services.Coupon.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Models
{
    public class CouponUpdateDTO : CouponCreateDTO, IUpdateDTO
    {
        public Guid Id { get; set; }
    }
}
