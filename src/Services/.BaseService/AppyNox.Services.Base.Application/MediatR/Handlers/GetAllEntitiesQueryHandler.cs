using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers
{
    public class GetAllEntitiesQueryHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork)
        : BaseHandler<TEntity>(repository, mapper, dtoMappingRegistry, serviceProvider, logger, unitOfWork),
        IRequestHandler<GetAllEntitiesQuery<TEntity>, IEnumerable<object>>
        where TEntity : class, IEntityTypeId
    {
        #region [ Public Methods ]

        public async Task<IEnumerable<object>> Handle(GetAllEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation($"Fetching entities of type '{typeof(TEntity).Name}'");
                var (expression, dtoType) = CreateProjection(request.QueryParameters);
                IEnumerable<object> entities = await Repository.GetAllAsync(request.QueryParameters, expression);
                List<object> resultList = MapEntitiesToDto(entities, dtoType, request.QueryParameters);
                return resultList;
            }
            catch (Exception ex) when (ex is INoxException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error occurred while fetching entities of type '{typeof(TEntity).Name}'");
                throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericGetAllQueryError);
            }
        }

        #endregion
    }
}