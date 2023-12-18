using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Extended;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Mappings
{
    public class IdentityRoleMapping : Profile
    {
        #region [ Public Constructors ]

        public IdentityRoleMapping()
        {
            CreateMap<IdentityRoleCreateDto, IdentityRole>();
            CreateMap<IdentityRoleDto, IdentityRole>().ReverseMap();
            CreateMap<IdentityRoleUpdateDto, IdentityRole>();
            CreateMap<IdentityRole, IdentityRoleWithClaimsDto>();
            CreateMap<IdentityRole, IdentityRoleWithAllPropertiesDto>();
        }

        #endregion
    }
}