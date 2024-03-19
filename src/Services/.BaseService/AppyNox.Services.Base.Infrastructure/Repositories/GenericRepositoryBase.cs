using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

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
    /// Retrieves an entity asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <param name="dtoType">The type of DTO (Data Transfer Object) to project the entity into.</param>
    /// <returns>A task representing the asynchronous operation, returning the retrieved entity.</returns>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown when the entity with the specified ID is not found.</exception>
    /// <exception cref="NoxInfrastructureException">Thrown when there is an error retrieving the entity from the database.</exception>
    public async Task<object> GetByIdAsync(Guid id, Type dtoType)
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            object? entity = await _dbSet.Where("Id == @0", id).Select(CreateProjection(dtoType)).AsNoTracking().FirstOrDefaultAsync();

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

    /// <summary>
    /// Retrieves an entity asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, returning the retrieved entity.</returns>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown when the entity with the specified ID is not found.</exception>
    /// <exception cref="NoxInfrastructureException">Thrown when there is an error retrieving the entity from the database.</exception>
    public async Task<TEntity> GetEntityByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            TEntity? entity = await _dbSet.Where("Id == @0", id).AsNoTracking().FirstOrDefaultAsync();

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

            var entities = await _dbSet
                .AsQueryable()
                .AsNoTracking()
                .Select(CreateProjection(dtoType))
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
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public void Update(TEntity entity)
    {
        try
        {
            _logger.LogInformation($"Attempting to update entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            _context.Set<TEntity>().Entry(entity).State = EntityState.Unchanged;

            if (!_context.Set<TEntity>().Local.Any(e => e == entity))
            {
                _context.Set<TEntity>().Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;

            _logger.LogInformation($"Entity marked as modified for update with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error attempting to mark entity for update with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.UpdatingDataError);
        }
    }

    /// <summary>
    /// Removes an entity of type TEntity from the repository.
    /// </summary>
    /// <param name="id">The id of the entity to remove.</param>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public async Task RemoveByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Attempting to delete entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            var entity = await _dbSet.FindAsync([id]);
            if (entity == null)
            {
                _logger.LogWarning($"Entity with ID: '{id}' not found.");
                throw new EntityNotFoundException<TEntity>(id);
            }
            _dbSet.Remove(entity);
            _logger.LogInformation($"Successfully deleted entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.DeletingDataError);
        }
    }

    #endregion

    #region [ Projection ]

    private static bool IsSimpleType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            // If it's a nullable type, check if the underlying type is simple.
            return IsSimpleType(Nullable.GetUnderlyingType(type)!);
        }

        if (type == typeof(Guid) || type == typeof(Guid?))
        {
            return true;
        }

        var typeCode = Type.GetTypeCode(type);
        switch (typeCode)
        {
            case TypeCode.Empty:
            case TypeCode.Object:
                return false;

            default:
                return true;
        }
    }

    private static Expression<Func<TEntity, object>> CreateProjection(Type dtoType)
    {
        var parameterExpr = Expression.Parameter(typeof(TEntity), "entity");
        bool checkNestedNavigation = false;
        HashSet<Type> visitedTypes = [];
        if (!dtoType.Name.Contains("Dto", StringComparison.OrdinalIgnoreCase))
        {
            checkNestedNavigation = true;
            visitedTypes = [dtoType];
        }
        var bindings = CreateBindings(parameterExpr, typeof(TEntity), dtoType, visitedTypes, checkNestedNavigation);

        var body = Expression.MemberInit(Expression.New(dtoType), bindings);
        return Expression.Lambda<Func<TEntity, object>>(body, parameterExpr);
    }

    private static IEnumerable<MemberBinding> CreateBindings(Expression source, Type sourceType, Type targetType, HashSet<Type> visitedTypes, bool checkNestedNavigation)
    {
        var bindings = new List<MemberBinding>();

        foreach (var targetProp in targetType.GetProperties())
        {
            if (targetProp.PropertyType == typeof(AuditInformation))
            {
                var auditInfoBindings = MapAuditInfoWithShadowProperties(source);
                var auditInfoInit = Expression.MemberInit(Expression.New(typeof(AuditInformation)), auditInfoBindings);
                bindings.Add(Expression.Bind(targetProp, auditInfoInit));
                continue;
            }

            var sourceProp = sourceType.GetProperty(targetProp.Name);
            if (sourceProp != null)
            {
                Expression propertyExpr = Expression.Property(source, sourceProp);
                MemberBinding binding;

                if (IsSimpleType(targetProp.PropertyType))
                {
                    binding = Expression.Bind(targetProp, propertyExpr);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(targetProp.PropertyType) && targetProp.PropertyType.IsGenericType)
                {
                    // Handle collections
                    var collectionType = targetProp.PropertyType.GetGenericArguments()[0];
                    var entityCollectionType = sourceProp.PropertyType.GetGenericArguments()[0];

                    var selectExpression = CreateCollectionSelectExpression(entityCollectionType, collectionType, visitedTypes, checkNestedNavigation);

                    // Create a call to Enumerable.Select
                    var selectCallExpression = Expression.Call(
                        typeof(Enumerable), nameof(Enumerable.Select),
                        [entityCollectionType, collectionType],
                        propertyExpr, selectExpression);

                    // Convert the IEnumerable result to a List (which implements ICollection)
                    var toListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList))!
                                                          .MakeGenericMethod(collectionType);
                    var toListCallExpression = Expression.Call(null, toListMethod, selectCallExpression);

                    // Bind the List result to the target property
                    binding = Expression.Bind(targetProp, toListCallExpression);
                }
                else // Complex type
                {
                    if (visitedTypes.Contains(targetProp.PropertyType))
                    {
                        continue;
                    }
                    visitedTypes.Add(targetProp.PropertyType);
                    var nestedBindings = CreateBindings(propertyExpr, sourceProp.PropertyType, targetProp.PropertyType, visitedTypes, checkNestedNavigation);
                    var nestedBody = Expression.MemberInit(Expression.New(targetProp.PropertyType), nestedBindings);
                    binding = Expression.Bind(targetProp, nestedBody);
                }

                bindings.Add(binding);
            }
        }

        return bindings;
    }

    private static LambdaExpression CreateCollectionSelectExpression(Type sourceType, Type targetType, HashSet<Type> visitedTypes, bool checkNestedNavigation)
    {
        var parameter = Expression.Parameter(sourceType, "x");
        var bindings = CreateBindings(parameter, sourceType, targetType, visitedTypes, checkNestedNavigation);
        var body = Expression.MemberInit(Expression.New(targetType), bindings);
        return Expression.Lambda(body, parameter);
    }

    private static IEnumerable<MemberBinding> MapAuditInfoWithShadowProperties(Expression source)
    {
        var auditPropertyNames = new[] { "CreatedBy", "CreationDate", "UpdatedBy", "UpdateDate" };
        var auditInfoType = typeof(AuditInformation);
        var bindings = new List<MemberBinding>();

        foreach (var propName in auditPropertyNames)
        {
            var targetProp = auditInfoType.GetProperty(propName);
            if (targetProp != null)
            {
                var propertyAccess = Expression.Call(
                    typeof(EF), nameof(EF.Property),
                    [targetProp.PropertyType],
                    source, Expression.Constant(propName));

                var binding = Expression.Bind(targetProp, propertyAccess);
                bindings.Add(binding);
            }
        }

        return bindings;
    }

    #endregion
}