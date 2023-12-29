using AppyNox.Services.Base.Domain.Common;

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

        #endregion
    }
}