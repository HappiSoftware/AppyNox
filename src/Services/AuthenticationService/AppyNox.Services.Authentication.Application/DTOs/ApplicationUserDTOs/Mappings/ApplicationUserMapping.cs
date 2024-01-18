using AppyNox.Services.Authentication.Application.DTOs.ApplicationUserDTOs.Models;
using AppyNox.Services.Authentication.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Authentication.Application.DTOs.ApplicationUserDTOs.Mappings
{
    /// <summary>
    /// Represents the mapping configuration for IdentityUser related DTOs and IdentityUser entity.
    /// Defines AutoMapper mappings to facilitate object conversions.
    /// </summary>
    public class ApplicationUserMapping : Profile
    {
        #region [ Public Constructors ]

        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUserCreateDto, ApplicationUser>();
            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUserUpdateDto, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserWithRolesDto>();
        }

        #endregion
    }
}