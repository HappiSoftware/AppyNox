using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Logger;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.MediatR.Handlers
{
    public class DeleteEntityCommandHandler<TEntity>(
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
            catch (Exception ex) when (ex is NoxInfrastructureException || ex is NoxApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error deleting entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
                throw new NoxApplicationException(ex);
            }
        }

        #endregion
    }
}