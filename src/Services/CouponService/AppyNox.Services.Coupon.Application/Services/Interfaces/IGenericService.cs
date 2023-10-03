using AppyNox.Services.Coupon.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Services.Interfaces
{
    public interface IGenericService<TEntity, TDto, TCreateDTO, TUpdateDTO>
    where TEntity : class
    where TDto : class
    where TCreateDTO : class
    where TUpdateDTO : class
    {
        Task<IEnumerable<dynamic>> GetAllAsync(QueryParameters queryParameters, string? detailLevel);

        Task<dynamic?> GetByIdAsync(Guid id, string? detailLevel = null);

        Task<(Guid guid, TDto basicDto)> AddAsync(TCreateDTO dto);

        Task UpdateAsync(TUpdateDTO dto);

        Task DeleteAsync(TDto dto);

    }
}
