using AutoMapper;
using CouponService.Application.DTOs.Coupon.Models;
using CouponService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Application.DTOs.Coupon.Mappings
{
    public class CouponMapping : Profile
    {
        public CouponMapping()
        {
            CreateMap<CouponCreateDTO, CouponEntity>();

            CreateMap<CouponUpdateDTO, CouponEntity>();

            CreateMap<CouponEntity, CouponDTO>();

            CreateMap<CouponEntity, CouponWithIdDTO>();

        }
    }
}
