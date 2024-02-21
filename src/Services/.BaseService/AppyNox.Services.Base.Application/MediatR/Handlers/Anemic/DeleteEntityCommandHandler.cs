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

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

public sealed class DeleteEntityCommandHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<DeleteEntityCommand<TEntity>>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly IGenericRepositoryBase<TEntity> _repository = repository;

    private readonly IUnitOfWorkBase _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Entity.Id}.");
            _repository.Remove(request.Entity);
            await _unitOfWork.SaveChangesAsync();
            await UpdateTotalCountOnCache(_cacheService, $"total-count-{typeof(TEntity).Name}", false);
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Entity.Id}.");
            throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericDeleteCommandError);
        }
    }

    #endregion
}