using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;

namespace AppyNox.Services.Base.Application.Interfaces.Repositories;

public interface INoxRepository<TEntity> where TEntity : class, IHasStronglyTypedId
{
    #region [ Public Methods ]

    Task<TEntity> AddAsync(TEntity entity);

    Task<PaginatedList<TEntity>> GetAllAsync(IQueryParameters queryParameters, ICacheService cacheService);

    Task<TEntity> GetByIdAsync<TId>(TId id) where TId : NoxId;

    Task RemoveByIdAsync<TId>(TId id) where TId : NoxId;

    void Update(TEntity entity);

    #endregion
}