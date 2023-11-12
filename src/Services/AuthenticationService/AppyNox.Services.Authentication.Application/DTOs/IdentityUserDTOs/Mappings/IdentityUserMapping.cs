using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Mappings
{
    public class IdentityUserMapping : Profile
    {
        #region [ Public Constructors ]

        public IdentityUserMapping()
        {
            CreateMap<IdentityUserCreateDto, IdentityUser>();
            CreateMap<IdentityUserDto, IdentityUser>().ReverseMap();
            CreateMap<IdentityUserUpdateDto, IdentityUser>();
            CreateMap<IdentityUser, IdentityUserWithRolesDto>();
            CreateMap<IdentityUser, IdentityUserWithAllPropertiesDto>();
        }

        #endregion
    }
}