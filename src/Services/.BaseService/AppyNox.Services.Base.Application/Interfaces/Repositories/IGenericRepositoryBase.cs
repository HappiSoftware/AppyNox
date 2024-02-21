using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Application.Interfaces.Repositories;

/// <summary>
/// Defines a generic repository interface for CRUD operations.
/// Used for Anemic Domain Modeling. If you are using Domain Driven Design, use
/// <see cref="INoxRepositoryBase{TEntity}"/> for entities with typed identifiers.
/// </summary>
/// <typeparam name="TEntity">The type of entity the repository manages.</typeparam>
public interface IGenericRepositoryBase<TEntity> where TEntity : class, IEntityWithGuid
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
    /// <param name="entity">The entity to remove.</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Retrieves all entities asynchronously based on specified query parameters and selected columns.
    /// </summary>
    /// <param name="queryParameters">The query parameters for filtering and pagination.</param>
    /// <param name="selectedColumns">The columns to include in the result. Created by CreateProjection. See more in <see cref="CreateProjection"/> </param>
    /// <returns>A collection of entities.</returns>
    Task<PaginatedList> GetAllAsync(IQueryParameters queryParameters, ICacheService cacheService);

    /// <summary>
    /// Retrieves an entity of type TEntity by its ID, selecting specific columns based on the provided expression.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="dtoType">An expression defining the columns to select for the entity.</param>
    /// <returns>The entity of type TEntity</returns>
    Task<TEntity> GetByIdAsync(Guid id);

    /// <summary>
    /// Updates an existing entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="properties">A list of property names to update.</param>
    void Update(TEntity entity, IList<string> properties);

    #endregion
}