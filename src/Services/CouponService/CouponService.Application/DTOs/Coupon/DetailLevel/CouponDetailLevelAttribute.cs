using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Application.DTOs.Coupon.DetailLevel
{
    public class CouponDetailLevelAttribute : Attribute
    {
        public CouponDetailLevel DetailLevel { get; }

        public CouponDetailLevelAttribute(CouponDetailLevel level)
        {
            DetailLevel = level;
        }

    }

    public enum CouponDetailLevel
    {
        [Description("Basic")]
        Basic,

        [Description("WithId")]
        WithId,

        [Description("WithAllProperties")]
        WithAllProperties
    }
}
