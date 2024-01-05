using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Logger;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AutoMapper;
using MediatR;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR.Handlers
{
    public class UpdateEntityCommandHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork)
        : BaseHandler<TEntity>(repository, mapper, dtoMappingRegistry, serviceProvider, logger, unitOfWork),
        IRequestHandler<UpdateEntityCommand<TEntity>>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        public async Task Handle(UpdateEntityCommand<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                #region [ Dynamic Dto Convertion ]

                Type dtoType = DtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.Update, typeof(TEntity), request.DetailLevel);
                dynamic dtoObject = JsonSerializer.Deserialize(request.Dto, dtoType, options: JsonSerializerOptions);

                Logger.LogInformation($"Updating entity ID: '{request.Id}' type '{typeof(TEntity).Name}'");

                #endregion

                #region [ FluentValidation ]

                FluentValidate(dtoType, dtoObject);

                #endregion

                TEntity mappedEntity = Mapper.Map(dtoObject, dtoType, typeof(TEntity));
                List<string> propertyList = (dtoObject as object).GetType().GetProperties().Select(x => x.Name).ToList();
                Repository.Update(mappedEntity, propertyList);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is NoxInfrastructureException || ex is NoxApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error updating entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
                throw new NoxApplicationException(ex);
            }
        }

        #endregion
    }
}