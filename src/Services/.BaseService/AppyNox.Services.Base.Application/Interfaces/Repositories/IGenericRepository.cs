using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Application.Interfaces.Repositories;

/// <summary>
/// Defines a generic repository interface for CRUD operations.
/// Used for Anemic Domain Modeling. If you are using Domain Driven Design, use
/// <see cref="INoxRepository{TEntity}"/> for entities with typed identifiers.
/// </summary>
/// <typeparam name="TEntity">The type of entity the repository manages.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
{
    #region [ CRUD Methods ]

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Removes an entity.
    /// </summary>
    /// <param name="id">The entity id to remove.</param>
    Task RemoveByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all entities asynchronously based on specified query parameters and selected columns.
    /// </summary>
    /// <param name="queryParameters">The query parameters for filtering and pagination.</param>
    /// /// <param name="cacheService">The cache service used for caching.</param>
    /// <returns>A collection of entities.</returns>
    Task<PaginatedList<TEntity>> GetAllAsync(IQueryParameters queryParameters, ICacheService cacheService);

    /// <summary>
    /// Retrieves an entity of type TEntity by its ID, selecting specific columns based on the provided expression.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The projectized object of TEntity </returns>
    Task<TEntity> GetByIdAsync(Guid id);

    /// <summary>
    /// Updates an existing entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    #endregion
}