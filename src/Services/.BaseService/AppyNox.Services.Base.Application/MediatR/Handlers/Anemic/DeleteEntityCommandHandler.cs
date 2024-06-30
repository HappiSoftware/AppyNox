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

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

internal sealed class DeleteEntityCommandHandler<TEntity>(
        IGenericRepository<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<DeleteEntityCommand<TEntity>>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly IGenericRepository<TEntity> _repository = repository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
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
            Logger.LogError(ex, $"Error deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericDeleteCommandError, innerException: ex);
        }
    }

    #endregion
}