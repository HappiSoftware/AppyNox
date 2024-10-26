﻿using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

internal sealed class DeleteNoxEntityCommandHandler<TEntity, TId>(
        INoxRepository<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger<DeleteNoxEntityCommandHandler<TEntity, TId>> logger,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider),
        IRequestHandler<DeleteNoxEntityCommand<TEntity, TId>>
        where TEntity : class, IHasStronglyTypedId
        where TId : NoxId
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly INoxRepository<TEntity> _repository = repository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task Handle(DeleteNoxEntityCommand<TEntity, TId> request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id.Value}.");
            await _repository.RemoveByIdAsync(request.Id, request.ForceDelete);
            await _unitOfWork.SaveChangesAsync();
            await UpdateTotalCountOnCache(_cacheService, $"total-count-{typeof(TEntity).Name}", false);
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id.Value}.");
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericDeleteCommandError, innerException: ex);
        }
    }

    #endregion
}