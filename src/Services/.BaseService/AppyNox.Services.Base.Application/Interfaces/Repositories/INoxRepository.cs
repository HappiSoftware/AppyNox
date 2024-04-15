using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Application.Interfaces.Repositories;

public interface INoxRepository<TEntity> where TEntity : class, IHasStronglyTypedId
{
    #region [ Public Methods ]

    Task<TEntity> AddAsync(TEntity entity);

    Task<PaginatedList<TEntity>> GetAllAsync(IQueryParameters queryParameters, ICacheService cacheService);

    Task<TEntity> GetByIdAsync<TId>(TId id) where TId : IHasGuidId;

    Task RemoveByIdAsync<TId>(TId id) where TId : IHasGuidId;

    void Update(TEntity entity);

    #endregion
}