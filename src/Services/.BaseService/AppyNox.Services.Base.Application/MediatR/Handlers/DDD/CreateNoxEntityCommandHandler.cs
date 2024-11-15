using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

internal sealed class CreateNoxEntityCommandHandler<TEntity, TDto>(
        INoxRepository<TEntity> repository,
        IMapper mapper,
        IServiceProvider serviceProvider,
        INoxApplicationLogger<CreateNoxEntityCommandHandler<TEntity, TDto>> logger,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, serviceProvider),
        IRequestHandler<CreateNoxEntityCommand<TEntity, TDto>, Guid>
        where TEntity : class, IHasStronglyTypedId
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly INoxRepository<TEntity> _repository = repository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task<Guid> Handle(CreateNoxEntityCommand<TEntity, TDto> request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Adding new entity of type '{typeof(TEntity).Name}'");

            FluentValidate(request.Dto);

            TEntity mappedEntity = Mapper.Map<TEntity>(request.Dto);
            await _repository.AddAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();
            await UpdateTotalCountOnCache(_cacheService, $"total-count-{typeof(TEntity).Name}", true);
            return mappedEntity.GetTypedId();
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericCreateCommandError, innerException: ex);
        }
    }

    #endregion
}