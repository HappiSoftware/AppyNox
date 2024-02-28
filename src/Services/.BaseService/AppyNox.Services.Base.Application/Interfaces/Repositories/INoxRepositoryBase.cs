using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Application.Interfaces.Repositories;

public interface INoxRepositoryBase<TEntity> where TEntity : class, IHasStronglyTypedId
{
    #region Public Methods

    Task<TEntity> AddAsync(TEntity entity);

    Task<PaginatedList> GetAllAsync(IQueryParameters queryParameters, Type dtoType, ICacheService cacheService);

    Task<object> GetByIdAsync<TId>(TId id, Type dtoType) where TId : IHasGuidId;

    Task<TEntity> GetEntityByIdAsync<TId>(TId id) where TId : IHasGuidId;

    Task RemoveByIdAsync<TId>(TId id) where TId : IHasGuidId;

    void Update(TEntity entity);

    #endregion
}