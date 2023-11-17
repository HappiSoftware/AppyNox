using AppyNox.Services.Coupon.Domain.Common;

namespace AppyNox.Services.Coupon.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        Task<TEntity> GetByIdAsync(Guid id);

        Task<IEnumerable<object>> GetAllAsync(QueryParameters queryParameters, Type dtoType);

        Task<TEntity> AddAsync(TEntity entity);

        void UpdateAsync(TEntity entity);

        void DeleteAsync(TEntity entity);

        #endregion
    }
}