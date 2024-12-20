﻿using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Data;
using AppyNox.Services.Base.Infrastructure.Exceptions;
using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Net;
using static AppyNox.Services.Base.Infrastructure.Repositories.RepositoryHelpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AppyNox.Services.Base.Infrastructure.Repositories;

/// <summary>
/// Defines a generic repository abstract class for CRUD operations.
/// </summary>
/// <typeparam name="TEntity">The type of entity the repository manages.</typeparam>
public abstract class GenericRepositoryBase<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly NoxDatabaseContext _context;

    private readonly DbSet<TEntity> _dbSet;

    private readonly INoxInfrastructureLogger<GenericRepositoryBase<TEntity>> _logger;

    private readonly string _countCacheKey = $"{NoxContext.CompanyId}-total-count-{typeof(TEntity).Name}";

    #endregion

    #region [ Protected Constructors ]

    protected GenericRepositoryBase(NoxDatabaseContext context, INoxInfrastructureLogger<GenericRepositoryBase<TEntity>> logger)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
        _logger = logger;
    }

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Retrieves an entity asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, returning the retrieved entity.</returns>
    /// <exception cref="NoxEntityNotFoundException{TEntity}">Thrown when the entity with the specified ID is not found.</exception>
    /// <exception cref="NoxInfrastructureException">Thrown when there is an error retrieving the entity from the database.</exception>
    public async Task<TEntity> GetByIdAsync(Guid id, bool includeDeleted = false, bool track = false)
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");

            IQueryable<TEntity> query = _dbSet;

            if (!track)
            {
                query = query.AsNoTracking();
            }
            if (includeDeleted)
            {
                ConfigureContext(new QueryParameters() { IncludeDeleted = includeDeleted });
            }

            TEntity? entity = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                _logger.LogWarning($"Entity with ID: {id} not found.");

                throw new NoxEntityNotFoundException<TEntity>(id);
            }

            _logger.LogInformation($"Successfully retrieved entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            return entity;
        }
        catch (Exception ex) when (ex is INoxInfrastructureException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(exceptionCode: (int)NoxInfrastructureExceptionCode.DataFetchingError, innerException: ex);
        }
    }

    /// <summary>
    /// Retrieves a paginated list of entities asynchronously based on the specified query parameters.
    /// </summary>
    /// <param name="queryParameters">The query parameters specifying pagination details.</param>
    /// <param name="cacheService">The cache service used for caching.</param>
    /// <returns>A task representing the asynchronous operation, returning a PaginatedList of entities.</returns>
    /// <exception cref="NoxInfrastructureException">Thrown when there is an error retrieving entities from the database.</exception>
    public async Task<PaginatedList<TEntity>> GetAllAsync(IQueryParameters queryParameters, ICacheService cacheService)
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entities. Type: '{typeof(TEntity).Name}'.");

            ConfigureContext(queryParameters);

            var query = _dbSet
            .AsQueryable()
            .AsNoTracking();                       

            // Validate and apply sorting
            if (!string.IsNullOrWhiteSpace(queryParameters.SortBy) && IsValidExpression(queryParameters.SortBy, _logger))
            {
                query = query.OrderBy(queryParameters.SortBy);
            }

            // Validate and apply filtering
            if (!string.IsNullOrWhiteSpace(queryParameters.Filter) && IsValidExpression(queryParameters.Filter, _logger))
            {
                query = query.Where(queryParameters.Filter);
            }

            // Try to get the count from cache
            var cachedCount = await cacheService.GetCachedValueAsync(_countCacheKey);
            if (!int.TryParse(cachedCount, out int totalCount))
            {
                // Count not in cache or invalid, so retrieve from database and cache it
                totalCount = await query.CountAsync();
                await cacheService.SetCachedValueAsync(_countCacheKey, totalCount.ToString(), TimeSpan.FromSeconds(20));
            }

            var entities = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            _logger.LogInformation($"Successfully retrieved entities. Type: '{typeof(TEntity).Name}'.");
            return new PaginatedList<TEntity>
            {
                Items = entities,
                ItemsCount = entities.Count,
                TotalCount = totalCount,
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex) when (ex is ParseException)
        {
            throw new NoxInfrastructureException("QueryParameter values were in wrong format.", (int)NoxInfrastructureExceptionCode.QueryParameterError, (int)HttpStatusCode.BadRequest, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving entities. Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(exceptionCode: (int)NoxInfrastructureExceptionCode.MultipleDataFetchingError, innerException: ex);
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
            throw new NoxInfrastructureException(exceptionCode: (int)NoxInfrastructureExceptionCode.AddingDataError, innerException: ex);
        }
    }

    /// <summary>
    /// Updates an existing entity of type TEntity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="changedPropertiesDto">The changed properties.</param>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public void Update(TEntity entity, dynamic changedPropertiesDto)
    {
        try
        {
            _logger.LogInformation($"Attempting to update entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            _context.Set<TEntity>().Entry(entity).State = EntityState.Unchanged;

            _context.Entry(entity).CurrentValues.SetValues(changedPropertiesDto);
            _context.Entry(entity).State = EntityState.Modified;

            _logger.LogInformation($"Entity marked as modified for update with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error attempting to mark entity for update with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(exceptionCode: (int)NoxInfrastructureExceptionCode.UpdatingDataError, innerException: ex);
        }
    }

    /// <summary>
    /// Removes an entity of type TEntity from the repository.
    /// </summary>
    /// <param name="id">The id of the entity to remove.</param>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public async Task RemoveByIdAsync(Guid id, bool forceDelete = false)
    {
        try
        {
            _logger.LogInformation($"Attempting to delete entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            var entity = await _dbSet.FindAsync([id]);
            if (entity == null)
            {
                _logger.LogWarning($"Entity with ID: '{id}' not found.");
                throw new NoxEntityNotFoundException<TEntity>(id);
            }

            if (forceDelete || entity is not ISoftDeletable)
            {
                // Hard delete
                _dbSet.Remove(entity);
                _logger.LogInformation($"Successfully hard deleted entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            }
            else
            {
                // Soft delete
                ((ISoftDeletable)entity).MarkAsDeleted();
                _dbSet.Update(entity);
                _logger.LogInformation($"Successfully soft deleted entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(exceptionCode: (int)NoxInfrastructureExceptionCode.DeletingDataError, innerException: ex);
        }
    }

    #endregion

    #region [ Private Methods ]

    private void ConfigureContext(IQueryParameters queryParameters)
    {
        // Apply include deleted
        _context.IgnoreSoftDeleteFilter = queryParameters.IncludeDeleted;
    }

    #endregion
}