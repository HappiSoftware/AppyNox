using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Infrastructure.Interfaces
{
    public interface IGenericRepositoryBase<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ CRUD Methods ]

        Task<TEntity> AddAsync(TEntity entity);

        void Remove(TEntity entity);

        Task<IEnumerable<object>> GetAllAsync(QueryParametersBase queryParameters, Expression<Func<TEntity, dynamic>> selectedColumns);

        Task<TEntity> GetByIdAsync(Guid id, Expression<Func<TEntity, dynamic>> selectedColumns);

        void Update(TEntity entity, IList<string> properties);

        #endregion

        #region [ Public Methods ]

        Expression<Func<TEntity, dynamic>> CreateProjection(List<string> propertyNames);

        #endregion
    }
}