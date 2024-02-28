using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

public class GetNoxEntityByIdQueryHandler<TEntity, TId>(
        INoxRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<GetNoxEntityByIdQuery<TEntity, TId>, object>
        where TEntity : class, IHasStronglyTypedId
        where TId : class, IHasGuidId
{
    #region [ Fields ]

    private readonly INoxRepositoryBase<TEntity> _repository = repository;

    #endregion

    #region [ Public Methods ]

    public async Task<object> Handle(GetNoxEntityByIdQuery<TEntity, TId> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {request.Id}.");
            var dtoType = GetDtoType(request.QueryParameters);
            object entity = await _repository.GetByIdAsync(request.Id, dtoType);
            return MapEntityToDtoSingle(entity, dtoType);
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error fetching entity with ID: {request.Id}.");
            throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericGetByIdQueryError);
        }
    }

    #endregion
}