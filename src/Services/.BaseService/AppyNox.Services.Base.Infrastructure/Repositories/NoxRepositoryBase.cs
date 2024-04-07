﻿using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Net;
using static AppyNox.Services.Base.Infrastructure.Repositories.RepositoryHelpers;

namespace AppyNox.Services.Base.Infrastructure.Repositories;

public abstract class NoxRepositoryBase<TEntity> : INoxRepository<TEntity> where TEntity : class, IHasStronglyTypedId
{
    #region [ Fields ]

    private readonly DbContext _context;

    private readonly DbSet<TEntity> _dbSet;

    private readonly INoxInfrastructureLogger _logger;

    private readonly string _countCacheKey = $"total-count-{typeof(TEntity).Name}";

    private static readonly char[] separator = [',', ' '];

    #endregion

    #region [ Protected Constructors ]

    protected NoxRepositoryBase(DbContext context, INoxInfrastructureLogger logger)
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
    /// <typeparam name="TId">The type of the entity's ID.</typeparam>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <param name="dtoType">The type of DTO (Data Transfer Object) to project the entity into.</param>
    /// <returns>A task representing the asynchronous operation, returning the retrieved entity.</returns>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown when the entity with the specified ID is not found.</exception>
    /// <exception cref="NoxInfrastructureException">Thrown when there is an error retrieving the entity from the database.</exception>

    public async Task<object> GetByIdAsync<TId>(TId id, Type dtoType)
        where TId : IHasGuidId
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            object? entity = await _dbSet.Where("Id == @0", id).Select(CreateProjection<TEntity>(dtoType)).AsNoTracking().FirstOrDefaultAsync();

            if (entity == null)
            {
                _logger.LogWarning($"Entity with ID: {id} not found.");

                throw new EntityNotFoundException<TEntity>(((IHasGuidId)id).GetGuidValue());
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

    /// <summary>
    /// Retrieves a paginated list of entities asynchronously based on the specified query parameters.
    /// </summary>
    /// <param name="queryParameters">The query parameters specifying pagination details.</param>
    /// <param name="dtoType">The type of DTO (Data Transfer Object) to project the entities into.</param>
    /// <param name="cacheService">The cache service used for caching.</param>
    /// <returns>A task representing the asynchronous operation, returning a PaginatedList of entities.</returns>
    /// <exception cref="NoxInfrastructureException">Thrown when there is an error retrieving entities from the database.</exception>
    public async Task<PaginatedList> GetAllAsync(IQueryParameters queryParameters, Type dtoType, ICacheService cacheService)
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

            var query = _dbSet
            .AsQueryable()
            .AsNoTracking();

            // Validate and apply sorting
            if (!string.IsNullOrWhiteSpace(queryParameters.SortBy) && IsValidExpression<TEntity>(queryParameters.SortBy))
            {
                query = query.OrderBy(queryParameters.SortBy);
            }

            // Validate and apply filtering
            if (!string.IsNullOrWhiteSpace(queryParameters.Filter) && IsValidExpression<TEntity>(queryParameters.Filter))
            {
                query = query.Where(queryParameters.Filter);
            }

            var entities = await query
                .Select(CreateProjection<TEntity>(dtoType))
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
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex) when (ex is ParseException)
        {
            throw new NoxInfrastructureException(ex, "QueryParameter values were in wrong format.", (int)NoxInfrastructureExceptionCode.QueryParameterError, (int)HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving entities. Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.MultipleDataFetchingError);
        }
    }

    /// <summary>
    /// Adds a new entity to the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add to the database.</param>
    /// <returns>A task representing the asynchronous operation, returning the added entity.</returns>
    /// <exception cref="NoxInfrastructureException">Thrown when there is an error adding the entity to the database.</exception>

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
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public void Update(TEntity entity)
    {
        try
        {
            _logger.LogInformation($"Attempting to update entity with ID: '{entity.GetTypedId}' Type: '{typeof(TEntity).Name}'.");
            _context.Set<TEntity>().Entry(entity).State = EntityState.Unchanged;

            if (!_context.Set<TEntity>().Local.Any(e => e == entity))
            {
                _context.Set<TEntity>().Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;

            _logger.LogInformation($"Entity marked as modified for update with ID: '{entity.GetTypedId}' Type: '{typeof(TEntity).Name}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error attempting to mark entity for update with ID: '{entity.GetTypedId}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.UpdatingDataError);
        }
    }

    /// <summary>
    /// Removes an entity of type TEntity from the repository.
    /// </summary>
    /// <param name="id">The id of the entity to remove.</param>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public async Task RemoveByIdAsync<TId>(TId id) where TId
        : IHasGuidId
    {
        try
        {
            _logger.LogInformation($"Attempting to delete entity with ID: '{id.GetGuidValue}' Type: '{typeof(TEntity).Name}'.");
            var entity = await _dbSet.FindAsync([id]);
            if (entity == null)
            {
                _logger.LogWarning($"Entity with ID: '{id}' not found.");
                throw new EntityNotFoundException<TEntity>(id.GetGuidValue());
            }
            _dbSet.Remove(entity);
            _logger.LogInformation($"Successfully deleted entity with ID: '{id.GetGuidValue}' Type: '{typeof(TEntity).Name}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting entity with ID: '{id.GetGuidValue}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.DeletingDataError);
        }
    }

    #endregion
}