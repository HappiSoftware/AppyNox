using AppyNox.Services.Coupon.Domain.Common;
using AppyNox.Services.Coupon.Domain.Interfaces;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.ExceptionExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq.Dynamic.Core;
using AppyNox.Services.Coupon.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ Fields ]

        private readonly CouponDbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        #endregion

        #region [ Public Constructors ]

        public GenericRepository(CouponDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }

        #endregion

        #region [ Public Methods ]

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.Where(item => item.Id == id).FirstOrDefaultAsync() ?? throw new EntityNotFoundException<TEntity>(id);
        }

        public async Task<IEnumerable<object>> GetAllAsync(QueryParameters queryParameters, Type dtoType)
        {
            return await _dbSet
                .AsQueryable()
                .Select(CreateProjection(dtoType))
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();
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

        #endregion

        #region [ Private Methods ]

        private static Expression<Func<TEntity, dynamic>> CreateProjection(Type dtoType)
        {
            // Create a list of property names from the DTO
            var propertyNames = dtoType.GetProperties().Select(p => p.Name).ToList();

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

        #endregion
    }
}