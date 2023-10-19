using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Domain.Interfaces;
using AppyNox.Services.Coupon.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Coupon.Domain.Common;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
    {
        private readonly CouponDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(CouponDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<dynamic> GetByIdAsync(Guid id, Type dtoType)
        {
            // Create a list of property names from the DTO
            var dtoPropertyNames = dtoType.GetProperties().Select(p => p.Name).ToList();

            // Query the database, select only the properties defined in the DTO
            var entity = await _dbSet
                .Where(e => e.Id == id)
                .Select(CreateProjection(dtoPropertyNames))
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new EntityNotFoundException<TEntity>(id);
            }

            return entity;
        }

        public async Task<IEnumerable<object>> GetAllAsync(QueryParameters queryParameters, Type dtoType)
        {
            var query = _dbSet.AsQueryable();

            query = ApplySearch(query, queryParameters);

            query = ApplySort(query, queryParameters);

            // Create a list of property names from the DTO
            var dtoPropertyNames = dtoType.GetProperties().Select(p => p.Name).ToList();

            // Use the Select method with the custom projection
            var resultList = await query
                .Select(CreateProjection(dtoPropertyNames))
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            return resultList;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public void UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        private IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, QueryParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SearchTerm) && !string.IsNullOrEmpty(parameters.SearchColumns))
            {
                var columnsToSearch = parameters.SearchColumns.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                // Initialize a predicate with no condition (always false)
                Expression<Func<TEntity, bool>> predicate = e => false;

                foreach (var column in columnsToSearch)
                {
                    var parameterExp = Expression.Parameter(typeof(TEntity), "type");
                    var propertyExp = Expression.Property(parameterExp, column);

                    if (propertyExp.Type == typeof(string))
                    {
                        // Search logic for string properties
                        MethodInfo? method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        if (method is null)
                        {
                            throw new InvalidOperationException("Unable to find the 'Contains' method on the string type.");
                        }
                        var someValue = Expression.Constant(parameters.SearchTerm, typeof(string));
                        var containsMethodExp = Expression.Call(propertyExp, method, someValue);

                        var lambda = Expression.Lambda<Func<TEntity, bool>>(containsMethodExp, parameterExp);
                        predicate = predicate.Or(lambda);
                    }
                    else if (propertyExp.Type == typeof(Guid))
                    {
                        // Search logic for Guid properties
                        if (Guid.TryParse(parameters.SearchTerm, out var searchGuid))
                        {
                            var guidEqualityExp = Expression.Equal(propertyExp, Expression.Constant(searchGuid));
                            var lambda = Expression.Lambda<Func<TEntity, bool>>(guidEqualityExp, parameterExp);
                            predicate = predicate.Or(lambda);
                        }
                    }
                    else if (propertyExp.Type == typeof(int))
                    {
                        // Search logic for int properties
                        if (int.TryParse(parameters.SearchTerm, out var searchInt))
                        {
                            var intEqualityExp = Expression.Equal(propertyExp, Expression.Constant(searchInt));
                            var lambda = Expression.Lambda<Func<TEntity, bool>>(intEqualityExp, parameterExp);
                            predicate = predicate.Or(lambda);
                        }
                    }
                    else if (propertyExp.Type == typeof(DateTime))
                    {
                        // Search logic for DateTime properties
                        if (DateTime.TryParse(parameters.SearchTerm, out var searchDateTime))
                        {
                            var dateTimeEqualityExp = Expression.Equal(propertyExp, Expression.Constant(searchDateTime));
                            var lambda = Expression.Lambda<Func<TEntity, bool>>(dateTimeEqualityExp, parameterExp);
                            predicate = predicate.Or(lambda);
                        }
                    }
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<TEntity> ApplySort(IQueryable<TEntity> query, QueryParameters parameters)
        {
            // Apply sorting
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                // Get the property with the specified name using reflection from TEntity
                var property = typeof(TEntity).GetProperty(parameters.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    // Build the sorting expression dynamically
                    var parameter = Expression.Parameter(typeof(TEntity), "x");
                    var propertyAccess = Expression.Property(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);

                    // Determine the sort direction
                    var methodName = parameters.SortOrder.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";

                    // Use reflection to get the appropriate method
                    var queryableType = typeof(Queryable);
                    var method = queryableType.GetMethods()
                        .Where(m => m.Name == methodName)
                        .Single(m => m.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(TEntity), property.PropertyType);

                    // Apply the sorting
                    query = (IQueryable<TEntity>)method.Invoke(null, new object[] { query, orderByExp });
                }
            }
            return query;
        }

        private Expression<Func<TEntity, dynamic>> CreateProjection(List<string> propertyNames)
        {
            var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
            var memberBindings = new List<MemberBinding>();

            foreach (var propertyName in propertyNames)
            {
                var entityProperty = Expression.Property(entityParameter, propertyName);

                // Make sure the property types match
                var conversion = Expression.Convert(entityProperty, entityProperty.Type);

                // Create a binding for each property
                var memberBinding = Expression.Bind(entityProperty.Member, conversion);
                memberBindings.Add(memberBinding);
            }

            // Create a MemberInitExpression without a constructor
            var memberInit = Expression.MemberInit(Expression.New(typeof(TEntity)), memberBindings);
            var selector = Expression.Lambda<Func<TEntity, dynamic>>(memberInit, entityParameter);

            return selector;
        }

    }
}
