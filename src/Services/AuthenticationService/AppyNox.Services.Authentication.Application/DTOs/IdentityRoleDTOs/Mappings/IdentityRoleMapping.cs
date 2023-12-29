using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base;
using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Extended;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Mappings
{
    /// <summary>
    /// Represents the mapping configuration for various IdentityRole related DTOs and IdentityRole entity.
    /// This class defines the AutoMapper mappings for these types to facilitate object conversions.
    /// </summary>
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