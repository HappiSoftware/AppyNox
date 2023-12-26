using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;

using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Infrastructure.Repositories
{
    public abstract class GenericRepositoryBase<TEntity> : IGenericRepositoryBase<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ Fields ]

        private readonly DbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        #endregion

        #region [ Protected Constructors ]

        protected GenericRepositoryBase(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }

        #endregion

        #region [ Public Methods ]

        public async Task<TEntity> GetByIdAsync(Guid id, Expression<Func<TEntity, dynamic>> selectedColumns)
        {
            return await _dbSet.Where(item => item.Id == id).Select(selectedColumns).FirstOrDefaultAsync() ?? throw new EntityNotFoundException<TEntity>(id);
        }

        public async Task<IEnumerable<object>> GetAllAsync(QueryParametersBase queryParameters, Expression<Func<TEntity, dynamic>> selectedColumns)
        {
            return await _dbSet
                .AsQueryable()
                .Select(selectedColumns)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public void UpdateAsync(TEntity entity, IList<string> properties)
        {
            _context.Set<TEntity>().Entry(entity).State = EntityState.Unchanged;
            properties = properties.Where(p => p != nameof(entity.Id)).ToList();
            foreach (var property in properties)
            {
                _context.Entry(entity).Property(property).IsModified = true;
            }
        }

        public void DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        #endregion

        #region [ Public Helper Methods ]

        public Expression<Func<TEntity, dynamic>> CreateProjection(List<string> propertyNames)
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

        #endregion
    }
}