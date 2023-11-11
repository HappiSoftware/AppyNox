using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Mappings
{
    public class IdentityRoleMapping : Profile
    {
        #region [ Public Constructors ]

        public IdentityRoleMapping()
        {
            CreateMap<IdentityRoleCreateDTO, IdentityRole>();
            CreateMap<IdentityRoleDTO, IdentityRole>().ReverseMap();
            CreateMap<IdentityRoleUpdateDTO, IdentityRole>();
            CreateMap<IdentityRole, IdentityRoleWithClaimsDTO>();
            CreateMap<IdentityRole, IdentityRoleWithAllPropertiesDTO>();
        }

        #endregion
    }
}