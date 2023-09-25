using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Mappings
{
    public class IdentityUserMapping : Profile
    {
        public IdentityUserMapping()
        {
            CreateMap<IdentityUserCreateDTO, IdentityUser>();
            CreateMap<IdentityUserDTO, IdentityUser>().ReverseMap();
            CreateMap<IdentityUserUpdateDTO, IdentityUser>();
            CreateMap<IdentityUser, IdentityUserWithRolesDTO>();
            CreateMap<IdentityUser, IdentityUserWithAllPropertiesDTO>();
        }
    }
}
