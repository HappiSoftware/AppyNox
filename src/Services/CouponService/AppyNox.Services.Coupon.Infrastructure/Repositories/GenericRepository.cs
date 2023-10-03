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

            // Apply sorting
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                // (mustang) apply sorting mechanism
                //query = queryParameters.SortOrder.ToLower() == "asc"
                //    ? query.OrderBy(queryParameters.SortBy)
                //    : query.OrderByDescending(queryParameters.SortBy);
            }

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

                var properties = typeof(TEntity).GetProperties();

                // Initialize a predicate with no condition (always false)
                Expression<Func<TEntity, bool>> predicate = e => false;

                foreach (var property in properties)
                {
                    if (columnsToSearch.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        var parameterExp = Expression.Parameter(typeof(TEntity), "type");
                        var propertyExp = Expression.Property(parameterExp, property.Name);
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
                }

                query = query.Where(predicate);
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
