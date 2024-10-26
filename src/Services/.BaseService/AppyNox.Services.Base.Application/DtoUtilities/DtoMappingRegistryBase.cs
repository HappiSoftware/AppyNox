﻿using AppyNox.Services.Base.Application.Exceptions;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;

namespace AppyNox.Services.Base.Application.DtoUtilities
{
    /// <summary>
    /// Provides a base implementation for a registry managing mappings between entities and their corresponding DTOs.
    /// </summary>
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

        public Dictionary<string, Type> GetDtoTypesForEntity(Type entityType, DtoLevelMappingTypes mappingType)
        {
            Dictionary<string, Type> result = [];

            if (_entityToDtoDetailLevelMappings.TryGetValue(entityType, out var entityAllDtoLevelMappings))
            {
                if (entityAllDtoLevelMappings.TryGetValue(mappingType, out var entityDtoLevelMappings))
                {
                    if (entityDtoLevelMappings is Type actualEnumType && actualEnumType.IsEnum)
                    {
                        foreach (Enum value in Enum.GetValues(actualEnumType))
                        {
                            var key = (entityType, mappingType, value);
                            if (_entityDetailLevelToDtoTypeMappings.TryGetValue(key, out var dtoType) && dtoType != null)
                            {
                                result.Add(value.GetDisplayName(), dtoType);
                            }
                        }
                    }
                    else
                    {
                        throw new DtoTypesForEntityException("The provided type is not an enum or is null.");
                    }
                }
                else
                {
                    throw new DtoTypesForEntityException("Mapping type not found.");
                }
            }
            else
            {
                throw new DtoTypesForEntityException("Entity type not found in mappings.");
            }

            return result;
        }

        #endregion

        #region [ Protected Methods ]

        /// <summary>
        /// Registers entity to DTO mappings. This method should be implemented in derived classes.
        /// </summary>
        protected abstract void RegisterDtos();

        /// <summary>
        /// Registers a mapping between an entity type and a DTO type.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="dtoType">The DTO type.</param>
        /// <param name="attribute">The attribute used to determine the detail level of the mapping.</param>
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
            var map = _entityToDtoDetailLevelMappings.GetValueOrDefault(entityType)
                ?? throw new AccessTypeNotFoundException(entityType, type.ToString());

            return map.GetValueOrDefault(type)
                ?? throw new AccessTypeNotFoundException(entityType, type.ToString());
        }

        #endregion
    }
}