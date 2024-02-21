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

public sealed class DeleteNoxEntityCommandHandler<TEntity, TId>(
        INoxRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<DeleteNoxEntityCommand<TEntity, TId>>
        where TEntity : class, IHasStronglyTypedId
        where TId : class, IHasGuidId
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly INoxRepositoryBase<TEntity> _repository = repository;

    private readonly IUnitOfWorkBase _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task Handle(DeleteNoxEntityCommand<TEntity, TId> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id.GetGuidValue}.");
            await _repository.RemoveByIdAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            await UpdateTotalCountOnCache(_cacheService, $"total-count-{typeof(TEntity).Name}", false);
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id.GetGuidValue}.");
            throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericDeleteCommandError);
        }
    }

    #endregion
}