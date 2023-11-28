using AppyNox.Services.Base.Domain.Common;

namespace AppyNox.Services.Base.Application.Services.Interfaces
{
    public interface IGenericServiceBase<TEntity, TDto>
    where TDto : class
    {
        #region [ Public Methods ]

        Task<IEnumerable<dynamic>> GetAllAsync(QueryParametersBase queryParameters);

        Task<dynamic?> GetByIdAsync(Guid id, QueryParametersBase queryParameters);

        Task<(Guid guid, TDto basicDto)> AddAsync(dynamic dto, string detailLevel);

        Task UpdateAsync(TDto dto);

        Task DeleteAsync(Guid dto);

        #endregion
    }
}