using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Mappings
{
    /// <summary>
    /// Represents the mapping configuration for IdentityUser related DTOs and IdentityUser entity.
    /// Defines AutoMapper mappings to facilitate object conversions.
    /// </summary>
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