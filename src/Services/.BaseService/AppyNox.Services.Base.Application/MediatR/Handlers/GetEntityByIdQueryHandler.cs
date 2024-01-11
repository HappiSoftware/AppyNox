using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Queries;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers
{
    public class GetEntityByIdQueryHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork)
        : BaseHandler<TEntity>(repository, mapper, dtoMappingRegistry, serviceProvider, logger, unitOfWork),
        IRequestHandler<GetEntityByIdQuery<TEntity>, object>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        public async Task<object> Handle(GetEntityByIdQuery<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {request.Id}.");
                var (expression, dtoType) = CreateProjection(request.QueryParameters);
                TEntity entity = await Repository.GetByIdAsync(request.Id, expression);
                return MapEntityToDtoSingle(entity, dtoType);
            }
            catch (Exception ex) when (ex is INoxInfrastructureException || ex is INoxApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error fetching entity with ID: {request.Id}.");
                throw new NoxApplicationException(ex);
            }
        }

        #endregion
    }
}