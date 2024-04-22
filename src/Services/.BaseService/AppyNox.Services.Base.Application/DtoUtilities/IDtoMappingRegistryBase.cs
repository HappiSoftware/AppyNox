using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Application.Exceptions;

namespace AppyNox.Services.Base.Application.DtoUtilities
{
    /// <summary>
    /// Defines the contract for a registry managing mappings between entities and their corresponding DTOs.
    /// </summary>
    public interface IDtoMappingRegistryBase
    {
        #region [ Public Methods ]

        /// <summary>
        /// Retrieves the DTO type associated with a given entity type and detail level.
        /// </summary>
        /// <param name="type">The DTO detail level type.</param>
        /// <param name="entityType">The entity type.</param>
        /// <returns>The DTO type associated with the specified entity and detail level.</returns>
        Type GetDetailLevelType(DtoLevelMappingTypes type, Type entityType);

        /// <summary>
        /// Gets the dictionary of detail level types for a specific entity type.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <returns>A dictionary mapping detail level types to DTO types for the specified entity.</returns>
        Dictionary<DtoLevelMappingTypes, Type> GetDetailLevelTypes(Type entityType);

        /// <summary>
        /// Retrieves the specific DTO type based on the detail level, entity type, and detail level description.
        /// </summary>
        /// <param name="detailLevelEnum">The DTO detail level enumeration.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="detailLevelDescription">The description of the detail level.</param>
        /// <returns>The specific DTO type.</returns>
        Type GetDtoType(DtoLevelMappingTypes detailLevelEnum, Type entityType, string detailLevelDescription);

        /// <summary>
        /// Retrieves a list of DTO types associated with a given entity type and mapping type.
        /// This method is used to dynamically determine the relevant data transfer object (DTO) types
        /// based on the operation context (like creating, reading, or updating an entity).
        /// </summary>
        /// <param name="entityType">The type of the entity for which DTO types are required.</param>
        /// <param name="mappingType">The mapping type that specifies the kind of operation context (e.g., DataAccess, Create, Update).</param>
        /// <returns>A dictionary of Type objects representing the DTOs as value and their enum Display name as key applicable to the specified entity type and mapping type.
        /// The dictionary contains only valid types and excludes null values. It may be empty if no applicable DTO types are found.</returns>
        /// <exception cref="DtoTypesForEntityException">Thrown when the entity type is not found in mappings, mapping type is not found, 
        /// or the provided type is not an enum or is null.</exception>
        Dictionary<string, Type> GetDtoTypesForEntity(Type entityType, DtoLevelMappingTypes mappingType);

        #endregion
    }
}