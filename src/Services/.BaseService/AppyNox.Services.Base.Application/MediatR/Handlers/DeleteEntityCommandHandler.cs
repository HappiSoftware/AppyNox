using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers
{
    public sealed class DeleteEntityCommandHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork)
        : BaseHandler<TEntity>(repository, mapper, dtoMappingRegistry, serviceProvider, logger, unitOfWork),
        IRequestHandler<DeleteEntityCommand<TEntity>>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        public async Task Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation($"Deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
                TEntity newEntity = Activator.CreateInstance<TEntity>();
                newEntity.Id = request.Id;
                Repository.Remove(newEntity);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is INoxException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
                throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericDeleteCommandError);
            }
        }

        #endregion
    }
}