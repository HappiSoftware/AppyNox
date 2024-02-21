using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

public sealed class CreateNoxEntityCommandHandler<TEntity>(
        INoxRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork,
        ICacheService cacheService)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<CreateNoxEntityCommand<TEntity>, (Guid guid, object basicDto)>
        where TEntity : class, IHasStronglyTypedId
{
    #region [ Fields ]

    private readonly ICacheService _cacheService = cacheService;

    private readonly INoxRepositoryBase<TEntity> _repository = repository;

    private readonly IUnitOfWorkBase _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task<(Guid guid, object basicDto)> Handle(CreateNoxEntityCommand<TEntity> request, CancellationToken cancellationToken)
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
            await _unitOfWork.SaveChangesAsync(NoxContext.UserId.ToString());
            await UpdateTotalCountOnCache(_cacheService, $"total-count-{typeof(TEntity).Name}", true);
            Type returnDtoType = DtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.DataAccess, entityType, CommonDtoLevelEnums.Simple.GetDisplayName());
            object createdObject = Mapper.Map(mappedEntity, returnDtoType, returnDtoType);
            return (guid: mappedEntity.GetTypedId, basicDto: createdObject);
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericCreateCommandError);
        }
    }

    #endregion
}