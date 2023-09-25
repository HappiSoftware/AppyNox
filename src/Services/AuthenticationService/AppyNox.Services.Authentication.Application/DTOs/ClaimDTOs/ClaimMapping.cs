using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs
{
    public class ClaimMapping : Profile
    {
        public ClaimMapping()
        {
            CreateMap<ClaimDTO, Claim>().ReverseMap();
        }
    }
}
