using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AppyNox.Services.Base.Infrastructure.Repositories;

/// <summary>
/// Defines a generic repository abstract class for CRUD operations.
/// </summary>
/// <typeparam name="TEntity">The type of entity the repository manages.</typeparam>
public abstract class GenericRepositoryBase<TEntity> : IGenericRepositoryBase<TEntity> where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly DbContext _context;

    private readonly DbSet<TEntity> _dbSet;

    private readonly INoxInfrastructureLogger _logger;

    private readonly string _countCacheKey = $"total-count-{typeof(TEntity).Name}";

    #endregion

    #region [ Protected Constructors ]

    protected GenericRepositoryBase(DbContext context, INoxInfrastructureLogger logger)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
        _logger = logger;
    }

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Retrieves an entity of type TEntity by its ID, selecting specific columns based on the provided expression.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="selectedColumns">An expression defining the columns to select for the entity.</param>
    /// <returns>The entity of type TEntity</returns>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown when TEntity is not found by given ID</exception>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            var entity = await _dbSet.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();

            if (entity == null)
            {
                _logger.LogWarning($"Entity with ID: {id} not found.");

                throw new EntityNotFoundException<TEntity>(id);
            }

            _logger.LogInformation($"Successfully retrieved entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            return entity;
        }
        catch (NoxInfrastructureException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.DataFetchingError);
        }
    }

    public async Task<PaginatedList> GetAllAsync(IQueryParameters queryParameters, ICacheService cacheService)
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entities. Type: '{typeof(TEntity).Name}'.");

            // Try to get the count from cache
            var cachedCount = await cacheService.GetCachedValueAsync(_countCacheKey);
            if (!int.TryParse(cachedCount, out int totalCount))
            {
                // Count not in cache or invalid, so retrieve from database and cache it
                totalCount = await _dbSet.CountAsync();
                await cacheService.SetCachedValueAsync(_countCacheKey, totalCount.ToString(), TimeSpan.FromMinutes(10));
            }

            var entities = await _dbSet
                .AsQueryable()
                .AsNoTracking()
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            _logger.LogInformation($"Successfully retrieved entities. Type: '{typeof(TEntity).Name}'.");
            return new PaginatedList
            {
                Items = entities,
                ItemsCount = entities.Count,
                TotalCount = totalCount,
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving entities. Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.MultipleDataFetchingError);
        }
    }

    /// <summary>
    /// Adds a new entity of type TEntity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity of type TEntity</returns>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            _logger.LogInformation($"Attempting to add a new entity. Type: '{typeof(TEntity).Name}'.");
            await _context.Set<TEntity>().AddAsync(entity);
            _logger.LogInformation($"Successfully added a new entity. Type: '{typeof(TEntity).Name}'.");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding a new entity. Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.AddingDataError);
        }
    }

    /// <summary>
    /// Updates an existing entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="properties">A list of property names to update.</param>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public void Update(TEntity entity, IList<string> properties)
    {
        try
        {
            _logger.LogInformation($"Attempting to update entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            _context.Set<TEntity>().Entry(entity).State = EntityState.Unchanged;

            // TODO With Value Objects, direct usage of "Id" can cause problems.
            properties = properties.Where(p => p != "Id").ToList();
            foreach (var property in properties)
            {
                _context.Entry(entity).Property(property).IsModified = true;
            }
            _logger.LogInformation($"Successfully updated entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.UpdatingDataError);
        }
    }

    /// <summary>
    /// Removes an entity of type TEntity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public void Remove(TEntity entity)
    {
        try
        {
            _logger.LogInformation($"Attempting to delete entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            _dbSet.Remove(entity);
            _logger.LogInformation($"Successfully deleted entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.DeletingDataError);
        }
    }

    #endregion
}