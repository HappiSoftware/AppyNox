using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Infrastructure.Interfaces
{
    public interface IGenericRepositoryBase<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        Task<TEntity> AddAsync(TEntity entity);

        Expression<Func<TEntity, dynamic>> CreateProjection(List<string> propertyNames);

        void DeleteAsync(TEntity entity);

        Task<IEnumerable<object>> GetAllAsync(QueryParametersBase queryParameters, Expression<Func<TEntity, dynamic>> selectedColumns);

        Task<TEntity> GetByIdAsync(Guid id, Expression<Func<TEntity, dynamic>> selectedColumns);

        void UpdateAsync(TEntity entity);

        #endregion
    }
}