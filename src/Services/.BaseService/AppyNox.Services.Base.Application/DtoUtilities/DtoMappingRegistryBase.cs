using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.DtoUtilities.ExceptionExtensions;
using AppyNox.Services.Base.Domain.Common;
using System.Data;
using System.Reflection;

namespace AppyNox.Services.Base.Application.DtoUtilities
{
    public abstract class DtoMappingRegistryBase
    {
        #region [ Fields ]

        private readonly Dictionary<(Type entityType, Enum detailLevel), Type> _entityDetailLevelToDtoTypeMappings;

        private readonly Dictionary<Type, Dictionary<DtoLevelMappingTypes, Type>> _entityToDtoDetailLevelMappings;

        #endregion

        #region [ Public Constructors ]

        public DtoMappingRegistryBase()
        {
            _entityDetailLevelToDtoTypeMappings = new Dictionary<(Type, Enum), Type>();
            _entityToDtoDetailLevelMappings = new Dictionary<Type, Dictionary<DtoLevelMappingTypes, Type>>();
            RegisterDtos();
        }

        #endregion

        #region [ Public Methods ]

        public abstract void RegisterDtos();

        public Type GetDtoType(Type detailLevelEnumType, Type entityType, string detailLevelDescription)
        {
            Enum detailLevel = AppyNoxEnumExtensions.GetEnumValueFromDisplayName(detailLevelEnumType, detailLevelDescription);

            if (_entityDetailLevelToDtoTypeMappings.TryGetValue((entityType, detailLevel), out var dtoType))
            {
                return dtoType;
            }

            throw new DetailLevelNotFoundException($"No Dto type mapping found for entity type {entityType} and detail level {detailLevel}.");
        }

        public Dictionary<DtoLevelMappingTypes, Type> GetDetailLevelTypes(Type entityType)
        {
            if (_entityToDtoDetailLevelMappings.TryGetValue(entityType, out var detailLevelMappings))
            {
                return detailLevelMappings;
            }

            throw new InvalidOperationException($"No detail level type found for entity type {entityType.FullName}.");
        }

        #endregion

        #region [ Private Methods ]

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
                mappings = new Dictionary<DtoLevelMappingTypes, Type>();
                _entityToDtoDetailLevelMappings.Add(entityType, mappings);
            }

            mappings[desiredMapping] = detailLevel.GetType();
            _entityDetailLevelToDtoTypeMappings[(entityType, detailLevel)] = dtoType;
        }

        protected abstract Enum GetDetailLevelBase(Attribute attribute, DtoLevelMappingTypes mappingType);

        #endregion
    }
}