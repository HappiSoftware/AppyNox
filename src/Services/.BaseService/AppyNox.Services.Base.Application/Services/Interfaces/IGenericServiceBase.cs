using AppyNox.Services.Base.Domain.Common;

namespace AppyNox.Services.Base.Application.Services.Interfaces
{
    public interface IGenericServiceBase<TEntity, TDto, TCreateDto, TUpdateDto>
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
    {
        #region [ Public Methods ]

        Task<IEnumerable<dynamic>> GetAllAsync(QueryParametersBase queryParameters);

        Task<dynamic?> GetByIdAsync(Guid id, QueryParametersBase queryParameters);

        Task<(Guid guid, TDto basicDto)> AddAsync(TCreateDto dto);

        Task UpdateAsync(TUpdateDto dto);

        Task DeleteAsync(TDto dto);

        #endregion
    }
}