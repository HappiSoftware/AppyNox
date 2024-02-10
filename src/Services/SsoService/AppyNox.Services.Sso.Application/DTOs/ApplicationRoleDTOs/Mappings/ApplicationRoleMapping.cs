using AppyNox.Services.Sso.Application.DTOs.ApplicationRoleDTOs.Models;
using AppyNox.Services.Sso.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationRoleDTOs.Mappings
{
    /// <summary>
    /// Represents the mapping configuration for various ApplicationRole related DTOs and ApplicationRole entity.
    /// This class defines the AutoMapper mappings for these types to facilitate object conversions.
    /// </summary>
    public class ApplicationRoleMapping : Profile
    {
        #region [ Public Constructors ]

        public ApplicationRoleMapping()
        {
            CreateMap<ApplicationRoleCreateDto, ApplicationRole>();
            CreateMap<ApplicationRoleDto, ApplicationRole>().ReverseMap();
            CreateMap<ApplicationRoleUpdateDto, ApplicationRole>();
            CreateMap<ApplicationRole, ApplicationRoleWithClaimsDto>();
        }

        #endregion
    }
}