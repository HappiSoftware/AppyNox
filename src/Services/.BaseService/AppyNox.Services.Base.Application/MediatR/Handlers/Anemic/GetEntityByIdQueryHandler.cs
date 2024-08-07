using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

internal class GetEntityByIdQueryHandler<TEntity>(
        IGenericRepository<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger<GetEntityByIdQueryHandler<TEntity>> logger)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider),
        IRequestHandler<GetEntityByIdQuery<TEntity>, object>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly IGenericRepository<TEntity> _repository = repository;

    private readonly IMapper _mapper = mapper;

    #endregion

    #region [ Public Methods ]

    public async Task<object> Handle(GetEntityByIdQuery<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {request.Id}.");
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
            logger.LogError(ex, $"Error fetching entity with ID: {request.Id}.");
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericGetByIdQueryError, innerException: ex);
        }
    }

    #endregion
}