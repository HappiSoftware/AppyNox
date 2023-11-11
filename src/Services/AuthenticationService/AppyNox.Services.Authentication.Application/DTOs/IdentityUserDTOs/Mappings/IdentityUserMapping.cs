using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Mappings
{
    public class IdentityUserMapping : Profile
    {
        #region [ Public Constructors ]

        public IdentityUserMapping()
        {
            CreateMap<IdentityUserCreateDTO, IdentityUser>();
            CreateMap<IdentityUserDTO, IdentityUser>().ReverseMap();
            CreateMap<IdentityUserUpdateDTO, IdentityUser>();
            CreateMap<IdentityUser, IdentityUserWithRolesDTO>();
            CreateMap<IdentityUser, IdentityUserWithAllPropertiesDTO>();
        }

        #endregion
    }
}