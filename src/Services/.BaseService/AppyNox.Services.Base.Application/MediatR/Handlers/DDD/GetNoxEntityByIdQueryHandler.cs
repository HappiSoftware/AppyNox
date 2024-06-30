using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

internal class GetNoxEntityByIdQueryHandler<TEntity, TId>(
        INoxRepository<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<GetNoxEntityByIdQuery<TEntity, TId>, object>
        where TEntity : class, IHasStronglyTypedId
        where TId : NoxId
{
    #region [ Fields ]

    private readonly INoxRepository<TEntity> _repository = repository;

    private readonly IMapper _mapper = mapper;

    #endregion

    #region [ Public Methods ]

    public async Task<object> Handle(GetNoxEntityByIdQuery<TEntity, TId> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {request.Id}.");
            var dtoType = GetDtoType(request.QueryParameters);
            var entity = await _repository.GetByIdAsync(request.Id, request.QueryParameters.IncludeDeleted, request.Track);
            return _mapper.Map(entity, typeof(TEntity), dtoType);
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error fetching entity with ID: {request.Id}.");
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericGetByIdQueryError, innerException: ex);
        }
    }

    #endregion
}