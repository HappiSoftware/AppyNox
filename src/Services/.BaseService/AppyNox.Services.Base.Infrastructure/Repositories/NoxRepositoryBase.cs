using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Infrastructure.Repositories;

public abstract class NoxRepositoryBase<TEntity> : INoxRepositoryBase<TEntity> where TEntity : class, IHasStronglyTypedId
{
    #region [ Fields ]

    private readonly DbContext _context;

    private readonly DbSet<TEntity> _dbSet;

    private readonly INoxInfrastructureLogger _logger;

    private readonly string _countCacheKey = $"total-count-{typeof(TEntity).Name}";

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
    /// Retrieves an entity of type TEntity by its ID, selecting specific columns based on the provided expression.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The entity of type TEntity</returns>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown when TEntity is not found by given ID</exception>
    /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
    public async Task<TEntity> GetByIdAsync<TId>(TId id)
        where TId : IHasGuidId
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
            var entity = await _dbSet.Where("Id == @0", id).AsNoTracking().FirstOrDefaultAsync();

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
    /// <param name="properties">A list of property names to update.</param>
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

    #region [ Public Helper Methods ]

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

    //private static List<MemberBinding> CreateBindings(Expression source, Type entityType, Type dtoType)
    //{
    //    var entityProperties = entityType.GetProperties();
    //    var dtoProperties = dtoType.GetProperties();
    //    var bindings = new List<MemberBinding>();

    //    foreach (var dtoProp in dtoProperties)
    //    {
    //        var entityProp = entityProperties.FirstOrDefault(ep => ep.Name == dtoProp.Name);
    //        if (entityProp != null)
    //        {
    //            if (NoxRepositoryBase<TEntity>.IsSimpleType(dtoProp.PropertyType))
    //            {
    //                if (entityProp.PropertyType == dtoProp.PropertyType)
    //                {
    //                    bindings.Add(Expression.Bind(dtoProp, Expression.Property(source, entityProp)));
    //                }
    //            }
    //            else if (typeof(IEnumerable).IsAssignableFrom(dtoProp.PropertyType) && dtoProp.PropertyType.IsGenericType)
    //            {
    //                // Handle collections
    //                var collectionType = dtoProp.PropertyType.GetGenericArguments()[0];
    //                var entityCollectionType = entityProp.PropertyType.GetGenericArguments()[0];

    //                var collectionParam = Expression.Parameter(entityCollectionType, "c");
    //                var collectionBindings = NoxRepositoryBase<TEntity>.CreateBindings(collectionParam, entityCollectionType, collectionType);
    //                var collectionBody = Expression.MemberInit(Expression.New(collectionType), collectionBindings);
    //                var selectExpression = Expression.Lambda(collectionBody, collectionParam);

    //                var propertyExpression = Expression.Property(source, entityProp);
    //                var selectMethod = typeof(Enumerable).GetMethods()
    //                                        .First(m => m.Name == "Select" && m.GetParameters().Length == 2)
    //                                        .MakeGenericMethod(entityCollectionType, collectionType);

    //                var callExpression = Expression.Call(selectMethod, propertyExpression, selectExpression);
    //                var convertExpression = Expression.Convert(callExpression, dtoProp.PropertyType);

    //                bindings.Add(Expression.Bind(dtoProp, convertExpression));
    //            }
    //            else
    //            {
    //                var subSource = Expression.Property(source, entityProp);
    //                var subBindings = NoxRepositoryBase<TEntity>.CreateBindings(subSource, entityProp.PropertyType, dtoProp.PropertyType);
    //                var subBody = Expression.MemberInit(Expression.New(dtoProp.PropertyType), subBindings);
    //                bindings.Add(Expression.Bind(dtoProp, subBody));
    //            }
    //        }
    //    }
    //    return bindings;
    //}

    //private static Expression<Func<TEntity, object>> CreateSelectExpression(Type dtoType)
    //{
    //    var parameter = Expression.Parameter(typeof(TEntity), "e");
    //    var bindings = NoxRepositoryBase<TEntity>.CreateBindings(parameter, typeof(TEntity), dtoType);

    //    var body = Expression.MemberInit(Expression.New(dtoType), bindings);
    //    return Expression.Lambda<Func<TEntity, object>>(body, parameter);
    //}

    private static Expression<Func<TEntity, object>> CreateProjection(Type dtoType)
    {
        var parameterExpr = Expression.Parameter(typeof(TEntity), "entity");
        var bindings = CreateBindings(parameterExpr, typeof(TEntity), dtoType);

        var body = Expression.MemberInit(Expression.New(dtoType), bindings);
        return Expression.Lambda<Func<TEntity, object>>(body, parameterExpr);
    }

    private static IEnumerable<MemberBinding> CreateBindings(Expression source, Type sourceType, Type targetType)
    {
        var bindings = new List<MemberBinding>();

        foreach (var targetProp in targetType.GetProperties())
        {
            var sourceProp = sourceType.GetProperty(targetProp.Name);
            if (sourceProp != null)
            {
                Expression propertyExpr = Expression.Property(source, sourceProp);
                MemberBinding binding;

                if (IsSimpleType(targetProp.PropertyType))
                {
                    binding = Expression.Bind(targetProp, propertyExpr);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(targetProp.PropertyType)
                     && targetProp.PropertyType.IsGenericType)
                {
                    // Handle collections
                    var collectionType = targetProp.PropertyType.GetGenericArguments()[0];
                    var entityCollectionType = sourceProp.PropertyType.GetGenericArguments()[0];

                    var selectExpression = CreateCollectionSelectExpression(entityCollectionType, collectionType);
                    var callExpression = Expression.Call(
                        typeof(Enumerable), "Select", new Type[] { entityCollectionType, collectionType },
                        propertyExpr, selectExpression);

                    binding = Expression.Bind(targetProp, callExpression);
                }
                else // Complex type
                {
                    var nestedBindings = CreateBindings(propertyExpr, sourceProp.PropertyType, targetProp.PropertyType);
                    var nestedBody = Expression.MemberInit(Expression.New(targetProp.PropertyType), nestedBindings);
                    binding = Expression.Bind(targetProp, nestedBody);
                }

                bindings.Add(binding);
            }
        }

        return bindings;
    }

    private static LambdaExpression CreateCollectionSelectExpression(Type sourceType, Type targetType)
    {
        var parameter = Expression.Parameter(sourceType, "x");
        var bindings = CreateBindings(parameter, sourceType, targetType);
        var body = Expression.MemberInit(Expression.New(targetType), bindings);
        return Expression.Lambda(body, parameter);
    }

    #endregion
}