using CouponService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class , IEntityWithGuid
    {
        Task<dynamic> GetByIdAsync(Guid id, Type dtoType);
        Task<IEnumerable<dynamic>> GetAllAsync(QueryParameters queryParameters, Type dtoType);
        Task<IEnumerable<T>> GetAllAsync(QueryParameters queryParameters);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
