using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel;
using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace AppyNox.Services.Authentication.Application.Dtos.DtoUtilities
{
    public class DtoMappingRegistry : DtoMappingRegistryBase
    {
        #region [ Public Methods ]

        public DtoMappingRegistry() : base()
        {
            RegisterDtos();
        }

        public static Enum GetDetailLevel(Attribute attribute, DtoLevelMappingTypes mappingType)
        {
            if (attribute is IdentityRoleDetailLevelAttribute identityRoleDetailAttribute)
            {
                return mappingType switch
                {
                    DtoLevelMappingTypes.DataAccess => identityRoleDetailAttribute.DataAccessDetailLevel,
                    DtoLevelMappingTypes.Create => identityRoleDetailAttribute.CreateDetailLevel,
                    DtoLevelMappingTypes.Update => identityRoleDetailAttribute.UpdateDetailLevel,
                    _ => identityRoleDetailAttribute.DataAccessDetailLevel
                };
            }
            else if (attribute is IdentityUserDetailLevelAttribute identityUserDetailAttribute)
            {
                return mappingType switch
                {
                    DtoLevelMappingTypes.DataAccess => identityUserDetailAttribute.DataAccessDetailLevel,
                    DtoLevelMappingTypes.Create => identityUserDetailAttribute.CreateDetailLevel,
                    DtoLevelMappingTypes.Update => identityUserDetailAttribute.UpdateDetailLevel,
                    _ => identityUserDetailAttribute.DataAccessDetailLevel
                };
            }

            throw new ArgumentException("Unsupported attribute type for detail level mapping.");
        }

        #endregion

        #region [ Protected Methods ]

        protected override void RegisterDtos()
        {
            var dtoTypes = Assembly.GetAssembly(typeof(IdentityUserDto))?
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains("Application.Dtos") && t.Namespace.Contains("Models"))
                .ToList();

            if (dtoTypes == null)
                return;

            foreach (var dtoType in dtoTypes)
            {
                var attributes = Attribute.GetCustomAttributes(dtoType);
                foreach (var attribute in attributes)
                {
                    if (attribute is IdentityRoleDetailLevelAttribute identityRoleAttribute)
                    {
                        RegisterMapping(typeof(IdentityRole), dtoType, identityRoleAttribute);
                    }
                    else if (attribute is IdentityUserDetailLevelAttribute identityUserAttribute)
                    {
                        RegisterMapping(typeof(IdentityRole), dtoType, identityUserAttribute);
                    }
                }
            }
        }

        protected override Enum GetDetailLevelBase(Attribute attribute, DtoLevelMappingTypes mappingType)
        {
            return GetDetailLevel(attribute, mappingType);
        }

        #endregion
    }
}