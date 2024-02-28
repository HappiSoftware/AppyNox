using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

internal class GetEntityByIdQueryHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<GetEntityByIdQuery<TEntity>, object>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly IGenericRepositoryBase<TEntity> _repository = repository;

    #endregion

    #region [ Public Methods ]

    public async Task<object> Handle(GetEntityByIdQuery<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {request.Id}.");
            var dtoType = GetDtoType(request.QueryParameters);
            return await _repository.GetByIdAsync(request.Id);
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