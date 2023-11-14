using AppyNox.Services.Coupon.Domain.Common;

namespace AppyNox.Services.Coupon.Application.Services.Interfaces
{
    public interface IGenericService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
    {
        #region [ Public Methods ]

        Task<IEnumerable<dynamic>> GetAllAsync(QueryParameters queryParameters, string detailLevel = "Basic");

        Task<dynamic?> GetByIdAsync(Guid id, string detailLevel = "Basic");

        Task<(Guid guid, TDto basicDto)> AddAsync(TCreateDto dto);

        Task UpdateAsync(TUpdateDto dto);

        Task DeleteAsync(TDto dto);

        #endregion
    }
}