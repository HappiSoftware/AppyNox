using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

internal sealed class CreateNoxEntityCommandHandler<TEntity>(
        INoxRepository<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<CreateNoxEntityCommand<TEntity>, Guid>
        where TEntity : class, IHasStronglyTypedId
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly INoxRepository<TEntity> _repository = repository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task<Guid> Handle(CreateNoxEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Adding new entity of type '{typeof(TEntity).Name}'");
            Type entityType = typeof(TEntity);

            #region [ Dynamic Dto Convertion ]

            Type dtoType = DtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.Create, entityType, request.DetailLevel);
            dynamic? dtoObject = JsonSerializer.Deserialize(request.Dto, dtoType, options: JsonSerializerOptions);

            #endregion

            #region [ FluentValidation ]

            FluentValidate(dtoType, dtoObject);

            #endregion

            TEntity mappedEntity = Mapper.Map(dtoObject, dtoType, entityType);
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