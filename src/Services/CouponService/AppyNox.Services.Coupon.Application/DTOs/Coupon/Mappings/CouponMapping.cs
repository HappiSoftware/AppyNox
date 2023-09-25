using AppyNox.Services.Coupon.Application.DTOs.Coupon.Models;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.DTOs.Coupon.Mappings
{
    public class CouponMapping : Profile
    {
        public CouponMapping()
        {
            CreateMap<CouponCreateDTO, CouponEntity>();

            CreateMap<CouponUpdateDTO, CouponEntity>();

            CreateMap<CouponEntity, CouponDTO>();

            CreateMap<CouponEntity, CouponWithIdDTO>().ReverseMap();

        }
    }
}
