using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Mappings
{
    public class IdentityRoleMapping : Profile
    {
        public IdentityRoleMapping()
        {
            CreateMap<IdentityRoleCreateDTO, IdentityRole>();
            CreateMap<IdentityRoleDTO, IdentityRole>().ReverseMap();
            CreateMap<IdentityRoleUpdateDTO, IdentityRole>();
            CreateMap<IdentityRole, IdentityRoleWithClaimsDTO>();
            CreateMap<IdentityRole, IdentityRoleWithAllPropertiesDTO>();
        }
    }
}
