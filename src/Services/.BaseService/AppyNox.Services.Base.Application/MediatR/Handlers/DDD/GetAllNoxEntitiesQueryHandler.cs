﻿using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

public class GetAllNoxEntitiesQueryHandler<TEntity>(
        INoxRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<GetAllNoxEntitiesQuery<TEntity>, PaginatedList>
        where TEntity : class, IHasStronglyTypedId
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly INoxRepositoryBase<TEntity> _repository = repository;

    #endregion

    #region [ Public Methods ]

    public async Task<PaginatedList> Handle(GetAllNoxEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Fetching entities of type '{typeof(TEntity).Name}'");
            var dtoType = GetDtoType(request.QueryParameters);
            PaginatedList paginatedList = await _repository.GetAllAsync(request.QueryParameters, dtoType, _cacheService);
            paginatedList.Items = MapEntitiesToDto(paginatedList.Items, dtoType, request.QueryParameters);
            return paginatedList;
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error occurred while fetching entities of type '{typeof(TEntity).Name}'");
            throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericGetAllQueryError);
        }
    }

    #endregion
}