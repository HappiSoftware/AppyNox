using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;

namespace AppyNox.Services.Base.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for a generic service handling CRUD operations for a specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the service.</typeparam>
    public interface IGenericServiceBase<TEntity>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        /// <summary>
        /// Retrieves all entities based on specified query parameters.
        /// </summary>
        /// <param name="queryParameters">The query parameters for filtering and pagination.</param>
        /// <returns>A collection of entities.</returns>
        Task<IEnumerable<dynamic>> GetAllAsync(QueryParametersBase queryParameters);

        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="queryParameters">The query parameters for detail level.</param>
        /// <returns>The requested entity.</returns>
        Task<dynamic> GetByIdAsync(Guid id, QueryParametersBase queryParameters);

        /// <summary>
        /// Adds a new entity based on the provided DTO and detail level.
        /// </summary>
        /// <param name="dto">The DTO representing the entity to add.</param>
        /// <param name="detailLevel">The detail level for the response.</param>
        /// <returns>A tuple containing the GUID and basic DTO of the added entity.</returns>
        Task<(Guid guid, dynamic basicDto)> AddAsync(dynamic dto, string detailLevel);

        /// <summary>
        /// Updates an existing entity based on the provided DTO and detail level.
        /// </summary>
        /// <param name="dto">The DTO representing the updates to the entity.</param>
        /// <param name="detailLevel">The detail level for the response.</param>
        Task UpdateAsync(dynamic dto, string detailLevel);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Gets the type of the entity managed by this service.
        /// </summary>
        /// <returns>The type of the managed entity.</returns>
        Type GetEntityType() => typeof(TEntity);

        #endregion
    }
}