using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using System.Data;
using System.Reflection;

namespace AppyNox.Services.Base.Application.DtoUtilities
{
    public abstract class DtoMappingRegistryBase : IDtoMappingRegistryBase
    {
        #region [ Fields ]

        private readonly Dictionary<(Type entityType, DtoLevelMappingTypes mappingType, Enum detailLevel), Type> _entityDetailLevelToDtoTypeMappings;

        private readonly Dictionary<Type, Dictionary<DtoLevelMappingTypes, Type>> _entityToDtoDetailLevelMappings;

        #endregion

        #region [ Protected Constructors ]

        protected DtoMappingRegistryBase()
        {
            _entityDetailLevelToDtoTypeMappings = [];
            _entityToDtoDetailLevelMappings = [];
        }

        #endregion

        #region [ Public Methods ]

        public Type GetDtoType(DtoLevelMappingTypes detailLevelEnum, Type entityType, string detailLevelDescription)
        {
            Enum detailLevel = NoxEnumExtensions.GetEnumValueFromDisplayName(GetDetailLevelType(detailLevelEnum, entityType), detailLevelDescription);

            if (_entityDetailLevelToDtoTypeMappings.TryGetValue((entityType, detailLevelEnum, detailLevel), out var dtoType))
            {
                return dtoType;
            }
            throw new DtoDetailLevelNotFoundException(entityType, detailLevel);
        }

        public Dictionary<DtoLevelMappingTypes, Type> GetDetailLevelTypes(Type entityType)
        {
            if (_entityToDtoDetailLevelMappings.TryGetValue(entityType, out var detailLevelMappings))
            {
                return detailLevelMappings;
            }

            throw new AccessTypeNotFoundException(entityType);
        }

        #endregion

        #region [ Protected Methods ]

        protected abstract void RegisterDtos();

        protected void RegisterMapping(Type entityType, Type dtoType, Attribute attribute)
        {
            string dtoName = dtoType.Name;
            DtoLevelMappingTypes desiredMapping = DtoLevelMappingTypes.DataAccess;

            if (dtoName.Contains("Create"))
            {
                desiredMapping = DtoLevelMappingTypes.Create;
            }
            else if (dtoName.Contains("Update"))
            {
                desiredMapping = DtoLevelMappingTypes.Update;
            }

            Enum detailLevel = GetDetailLevelBase(attribute, desiredMapping);

            if (!_entityToDtoDetailLevelMappings.TryGetValue(entityType, out var mappings))
            {
                mappings = [];
                _entityToDtoDetailLevelMappings.Add(entityType, mappings);
            }

            mappings[desiredMapping] = detailLevel.GetType();
            _entityDetailLevelToDtoTypeMappings[(entityType, desiredMapping, detailLevel)] = dtoType;
        }

        protected abstract Enum GetDetailLevelBase(Attribute attribute, DtoLevelMappingTypes mappingType);

        #endregion

        #region [ Private Methods ]

        public Type GetDetailLevelType(DtoLevelMappingTypes type, Type entityType)
        {
            var map = _entityToDtoDetailLevelMappings.GetValueOrDefault(entityType) ?? throw new AccessTypeNotFoundException(entityType, type.ToString());
            return map.GetValueOrDefault(type) ?? throw new AccessTypeNotFoundException(entityType, type.ToString());
        }

        #endregion
    }
}