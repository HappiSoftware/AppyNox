using AppyNox.Services.Coupon.Application.DTOs.Coupon.DetailLevel;
using AppyNox.Services.Coupon.Application.DTOs.Coupon.Models;
using AppyNox.Services.Coupon.Application.ExceptionExtensions;
using AppyNox.Services.Coupon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.DTOUtilities
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
            var dtoTypes = Assembly.GetAssembly(typeof(CouponDTO))?
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains("Application.DTOs") && t.Namespace.EndsWith("Models"))
                .ToList();

            if (dtoTypes == null)
                return;

            foreach (var dtoType in dtoTypes)
            {
                var attributes = Attribute.GetCustomAttributes(dtoType);
                foreach (var attribute in attributes)
                {
                    if (attribute is CouponDetailLevelAttribute couponAttribute)
                    {
                        RegisterMapping(typeof(CouponEntity), dtoType, couponAttribute.DetailLevel);
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
    }
}
