using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

internal class GetAllEntitiesQueryHandler<TEntity, TDto>(
        IGenericRepository<TEntity> repository,
        IMapper mapper,
        IServiceProvider serviceProvider,
        INoxApplicationLogger<GetAllEntitiesQueryHandler<TEntity, TDto>> logger,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, serviceProvider),
        IRequestHandler<GetAllEntitiesQuery<TEntity, TDto>, PaginatedList<TDto>>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly IGenericRepository<TEntity> _repository = repository;

    private readonly IMapper _mapper = mapper;

    #endregion

    #region [ Public Methods ]

    public async Task<PaginatedList<TDto>> Handle(GetAllEntitiesQuery<TEntity, TDto> request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Fetching entities of type '{typeof(TEntity).Name}'");
            var paginatedEntityList = await _repository.GetAllAsync(request.QueryParameters, _cacheService);
            var mappedItems = _mapper.Map<IEnumerable<TDto>>(paginatedEntityList.Items);

            return new PaginatedList<TDto>
            {
                Items = mappedItems!, // risky
                ItemsCount = paginatedEntityList.ItemsCount,
                TotalCount = paginatedEntityList.TotalCount,
                CurrentPage = paginatedEntityList.CurrentPage,
                PageSize = paginatedEntityList.PageSize
            };
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while fetching entities of type '{typeof(TEntity).Name}'");
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericGetAllQueryError, innerException: ex);
        }
    }

    #endregion
}