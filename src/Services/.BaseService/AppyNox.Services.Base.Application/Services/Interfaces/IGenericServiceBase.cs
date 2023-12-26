using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Application.Services.Interfaces
{
    public interface IGenericServiceBase<TEntity>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        Task<IEnumerable<dynamic>> GetAllAsync(QueryParametersBase queryParameters);

        Task<dynamic> GetByIdAsync(Guid id, QueryParametersBase queryParameters);

        Task<(Guid guid, dynamic basicDto)> AddAsync(dynamic dto, string detailLevel);

        Task UpdateAsync(dynamic dto);

        Task DeleteAsync(Guid id);

        Type GetEntityType() => typeof(TEntity);

        #endregion
    }
}