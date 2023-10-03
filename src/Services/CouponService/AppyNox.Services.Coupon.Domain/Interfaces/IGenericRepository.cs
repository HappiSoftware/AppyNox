using AppyNox.Services.Coupon.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntityWithGuid
    {
        Task<dynamic> GetByIdAsync(Guid id, Type dtoType);
        Task<IEnumerable<dynamic>> GetAllAsync(QueryParameters queryParameters, Type dtoType);
        Task<TEntity> AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TEntity entity);
    }
}
