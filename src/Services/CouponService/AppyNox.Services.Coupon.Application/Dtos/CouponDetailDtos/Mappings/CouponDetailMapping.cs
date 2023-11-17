using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Mappings
{
    public class CouponDetailMapping : Profile
    {
        #region [ Public Constructors ]

        public CouponDetailMapping()
        {
            CreateMap<CouponDetailEntity, CouponDetailSimpleDto>().ReverseMap();
        }

        #endregion
    }
}