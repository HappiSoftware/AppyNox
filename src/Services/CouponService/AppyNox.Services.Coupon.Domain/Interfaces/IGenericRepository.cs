using AppyNox.Services.Coupon.Domain.Common;

namespace AppyNox.Services.Coupon.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        Task<dynamic> GetByIdAsync(Guid id, Type dtoType);

        Task<IEnumerable<dynamic>> GetAllAsync(QueryParameters queryParameters, Type dtoType);

        Task<TEntity> AddAsync(TEntity entity);

        void UpdateAsync(TEntity entity);

        void DeleteAsync(TEntity entity);

        #endregion
    }
}