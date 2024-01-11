using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Extended;
using AppyNox.Services.Authentication.Domain.Entities;
using AutoMapper;

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
            CreateMap<IdentityUserCreateDto, ApplicationUser>();
            CreateMap<IdentityUserDto, ApplicationUser>().ReverseMap();
            CreateMap<IdentityUserUpdateDto, ApplicationUser>();
            CreateMap<ApplicationUser, IdentityUserWithRolesDto>();
            CreateMap<ApplicationUser, IdentityUserWithAllPropertiesDto>();
        }

        #endregion
    }
}