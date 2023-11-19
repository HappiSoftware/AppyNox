using AppyNox.Services.Coupon.Domain.Common;
using System.Linq.Expressions;

namespace AppyNox.Services.Coupon.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        Task<TEntity> AddAsync(TEntity entity);

        Expression<Func<TEntity, dynamic>> CreateProjection(List<string> propertyNames);

        void DeleteAsync(TEntity entity);

        Task<IEnumerable<object>> GetAllAsync(QueryParameters queryParameters, Expression<Func<TEntity, dynamic>> selectedColumns);

        Task<TEntity> GetByIdAsync(Guid id, Expression<Func<TEntity, dynamic>> selectedColumns);

        void UpdateAsync(TEntity entity);

        #endregion
    }
}