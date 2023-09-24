using CouponService.Domain.Common;
using CouponService.Domain.Interfaces;
using CouponService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using CouponService.Infrastructure.ExceptionExtensions;
using CouponService.Application.DTOs.Coupon.Models;
using AutoMapper.QueryableExtensions;

namespace CouponService.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class , IEntityWithGuid
    {
        private readonly CouponDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CouponDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
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
                throw new NotFoundException($"Entity with id {id} could not be found");
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

        [Obsolete("Deprecated")]
        public async Task<IEnumerable<T>> GetAllAsync(QueryParameters queryParameters)
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

            return await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private IQueryable<T> ApplySearch(IQueryable<T> query, QueryParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SearchTerm) && !string.IsNullOrEmpty(parameters.SearchColumns))
            {
                var columnsToSearch = parameters.SearchColumns.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                var properties = typeof(T).GetProperties();

                // Initialize a predicate with no condition (always false)
                Expression<Func<T, bool>> predicate = e => false;

                foreach (var property in properties)
                {
                    if (columnsToSearch.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        var parameterExp = Expression.Parameter(typeof(T), "type");
                        var propertyExp = Expression.Property(parameterExp, property.Name);
                        MethodInfo? method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        if (method is null)
                        {
                            throw new InvalidOperationException("Unable to find the 'Contains' method on the string type.");
                        }
                        var someValue = Expression.Constant(parameters.SearchTerm, typeof(string));
                        var containsMethodExp = Expression.Call(propertyExp, method, someValue);

                        var lambda = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
                        predicate = predicate.Or(lambda);
                    }
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private Expression<Func<T, dynamic>> CreateProjection(List<string> propertyNames)
        {
            var entityParameter = Expression.Parameter(typeof(T), "entity");
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
            var memberInit = Expression.MemberInit(Expression.New(typeof(T)), memberBindings);
            var selector = Expression.Lambda<Func<T, dynamic>>(memberInit, entityParameter);

            return selector;
        }

    }
}
