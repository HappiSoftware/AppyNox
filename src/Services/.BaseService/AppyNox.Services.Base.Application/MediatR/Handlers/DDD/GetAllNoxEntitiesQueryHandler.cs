using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.DtoUtilities;
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

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

internal class GetAllNoxEntitiesQueryHandler<TEntity>(
        INoxRepository<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<GetAllNoxEntitiesQuery<TEntity>, PaginatedList<object>>
        where TEntity : class, IHasStronglyTypedId
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly INoxRepository<TEntity> _repository = repository;

    private readonly IMapper _mapper = mapper;

    #endregion

    #region [ Public Methods ]

    public async Task<PaginatedList<object>> Handle(GetAllNoxEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Fetching entities of type '{typeof(TEntity).Name}'");
            var dtoType = GetDtoType(request.QueryParameters);
            var paginatedEntityList = await _repository.GetAllAsync(request.QueryParameters, _cacheService);
            var mappedItems = _mapper.Map(paginatedEntityList.Items, paginatedEntityList.Items.GetType(), typeof(IEnumerable<>).MakeGenericType(dtoType))
                as IEnumerable<object>;
            return new PaginatedList<object>
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
            Logger.LogError(ex, $"Error occurred while fetching entities of type '{typeof(TEntity).Name}'");
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericGetAllQueryError, innerException: ex);
        }
    }

    #endregion
}