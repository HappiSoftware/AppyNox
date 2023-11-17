using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.ExceptionExtensions;
using AppyNox.Services.Coupon.Domain.Common;
using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application.DtoUtilities
{
    public class DtoMappingRegistry
    {
        #region [ Fields ]

        private readonly Dictionary<(Type entityType, Enum detailLevel), Type> _entityDetailLevelToDtoTypeMappings;
        private readonly Dictionary<Type, Dictionary<DtoMappingTypes, Type>> _entityToDtoDetailLevelMappings;

        #endregion

        #region [ Public Constructors ]

        public DtoMappingRegistry()
        {
            _entityDetailLevelToDtoTypeMappings = new Dictionary<(Type, Enum), Type>();
            _entityToDtoDetailLevelMappings = new Dictionary<Type, Dictionary<DtoMappingTypes, Type>>();
            RegisterDtos();
        }

        #endregion

        #region [ Public Methods ]

        public void RegisterDtos()
        {
            var dtoTypes = Assembly.GetAssembly(typeof(CouponSimpleDto))?
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
                    if (attribute is CouponDetailLevelAttribute couponAttribute)
                    {
                        RegisterMapping(typeof(CouponEntity), dtoType, couponAttribute);
                    }
                }
            }
        }

        public Type GetDtoType(Type detailLevelEnumType, Type entityType, string detailLevelDescription)
        {
            Enum detailLevel = EnumExtensions.GetEnumValueFromDescription(detailLevelEnumType, detailLevelDescription);

            if (_entityDetailLevelToDtoTypeMappings.TryGetValue((entityType, detailLevel), out var dtoType))
            {
                return dtoType;
            }

            throw new DetailLevelNotFoundException($"No Dto type mapping found for entity type {entityType} and detail level {detailLevel}.");
        }

        public Dictionary<DtoMappingTypes, Type> GetDetailLevelTypes(Type entityType)
        {
            if (_entityToDtoDetailLevelMappings.TryGetValue(entityType, out var detailLevelMappings))
            {
                return detailLevelMappings;
            }

            throw new InvalidOperationException($"No detail level type found for entity type {entityType.FullName}.");
        }

        #endregion

        #region [ Private Methods ]

        private void RegisterMapping(Type entityType, Type dtoType, CouponDetailLevelAttribute attribute)
        {
            string dtoName = dtoType.Name;
            DtoMappingTypes desiredMapping = DtoMappingTypes.DataAccess;

            if (dtoName.Contains("Create"))
            {
                desiredMapping = DtoMappingTypes.Create;
            }
            else if (dtoName.Contains("Update"))
            {
                desiredMapping = DtoMappingTypes.Update;
            }

            Enum detailLevel = GetDetailLevel(attribute, desiredMapping);

            if (!_entityToDtoDetailLevelMappings.TryGetValue(entityType, out var mappings))
            {
                mappings = new Dictionary<DtoMappingTypes, Type>();
                _entityToDtoDetailLevelMappings.Add(entityType, mappings);
            }

            mappings[desiredMapping] = detailLevel.GetType();
            _entityDetailLevelToDtoTypeMappings[(entityType, detailLevel)] = dtoType;
        }

        private static Enum GetDetailLevel(CouponDetailLevelAttribute attribute, DtoMappingTypes mappingType)
        {
            return mappingType switch
            {
                DtoMappingTypes.DataAccess => attribute.DataAccessDetailLevel,
                DtoMappingTypes.Create => attribute.CreateDetailLevel,
                DtoMappingTypes.Update => attribute.UpdateDetailLevel,
                _ => attribute.DataAccessDetailLevel, // Default to DataAccess if none of the specific cases match
            };
        }

        #endregion
    }

}