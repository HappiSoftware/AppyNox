using AppyNox.Services.Coupon.Application.Dtos.Coupon.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.Coupon.Models;
using AppyNox.Services.Coupon.Application.ExceptionExtensions;
using AppyNox.Services.Coupon.Domain.Entities;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application.DtoUtilities
{
    public class DtoMappingRegistry
    {
        #region [ Fields ]

        private readonly Dictionary<(Type entity, Enum detailLevel), Type> _mappings;

        private readonly Dictionary<Type, Type> _entityEnumMappings;

        #endregion

        #region [ Public Constructors ]

        public DtoMappingRegistry()
        {
            _mappings = new Dictionary<(Type, Enum), Type>();
            _entityEnumMappings = new Dictionary<Type, Type>();
            RegisterDtos();
        }

        #endregion

        #region [ Public Methods ]

        public void RegisterDtos()
        {
            // Scan for Dtos in the Application assembly
            var dtoTypes = Assembly.GetAssembly(typeof(CouponDto))?
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains("Application.Dtos") && t.Namespace.EndsWith("Models"))
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

        public Type GetDtoType(Type detailLevelEnumType, Type entityType, string detailLevelDescription)
        {
            Enum level = EnumExtensions.GetEnumValueFromDescription(detailLevelEnumType, detailLevelDescription);
            if (_mappings.TryGetValue((entityType, level), out var dtoType))
            {
                return dtoType;
            }
            throw new DetailLevelNotFoundException($"No Dto type mapping found for entity type {entityType} and detail level {level}.");
        }

        public Type GetDetailLevelType(Type entityType)
        {
            if (_entityEnumMappings.TryGetValue(entityType, out Type? detailLevelType))
            {
                return detailLevelType;
            }
            throw new InvalidOperationException($"No detail level type found for entity type {entityType.FullName}.");
        }

        #endregion

        #region [ Private Methods ]

        private void RegisterMapping(Type entityType, Type dtoType, Enum level)
        {
            _mappings[(entityType, level)] = dtoType;
            if (!_entityEnumMappings.ContainsKey(entityType))
            {
                _entityEnumMappings.Add(entityType, level.GetType());
            }
        }

        #endregion
    }
}