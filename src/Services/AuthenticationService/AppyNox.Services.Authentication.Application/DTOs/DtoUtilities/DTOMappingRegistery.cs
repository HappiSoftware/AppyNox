using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel;
using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models;
using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel;
using AppyNox.Services.Authentication.Application.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace AppyNox.Services.Authentication.Application.DTOs.DtoUtilities
{
    public class DTOMappingRegistry
    {
        private readonly Dictionary<(Type entity, Enum detailLevel), Type> _mappings;
        private readonly IDictionary<Type, Type> _entityEnumMappings;

        public DTOMappingRegistry()
        {
            _mappings = new Dictionary<(Type, Enum), Type>();
            _entityEnumMappings = new Dictionary<Type, Type>();
            RegisterDTOs();
        }

        public void RegisterDTOs()
        {
            // Scan for DTOs in the Application assembly
            var dtoTypes = Assembly.GetAssembly(typeof(IdentityRoleDTO))?
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith("Authentication.Application.DTOs"))
                .ToList();

            if (dtoTypes == null)
                return;

            foreach (var dtoType in dtoTypes)
            {
                var attributes = Attribute.GetCustomAttributes(dtoType);
                foreach (var attribute in attributes)
                {
                    if (attribute is IdentityRoleDetailLevelAttribute roleDetailLevelAttribute)
                    {
                        RegisterMapping(typeof(IdentityRole), dtoType, roleDetailLevelAttribute.DetailLevel);
                    }
                    else if (attribute is IdentityUserDetailLevelAttribute identityUserDetailLevelAttribute)
                    {
                        RegisterMapping(typeof(IdentityUser), dtoType, identityUserDetailLevelAttribute.DetailLevel);
                    }
                }
            }
        }

        private void RegisterMapping(Type entityType, Type dtoType, Enum level)
        {
            _mappings[(entityType, level)] = dtoType;
            if (!_entityEnumMappings.ContainsKey(entityType))
            {
                _entityEnumMappings.Add(entityType, level.GetType());
            }
        }

        public Type GetDTOType(Type detailLevelEnumType, Type entityType, string detailLevelDescription)
        {
            Enum level = EnumExtensions.GetEnumValueFromDescription(detailLevelEnumType, detailLevelDescription);
            if (_mappings.TryGetValue((entityType, level), out var dtoType))
            {
                return dtoType;
            }
            throw new DetailLevelNotFoundException($"No DTO type mapping found for entity type {entityType} and detail level {level}.");
        }

        public Type GetDetailLevelType(Type entityType)
        {
            if (_entityEnumMappings.TryGetValue(entityType, out Type? detailLevelType))
            {
                return detailLevelType;
            }
            throw new InvalidOperationException($"No detail level type found for entity type {entityType.FullName}.");
        }

        public string GetBasicDetailLevel()
        {
            return "Basic";
        }
    }

}
