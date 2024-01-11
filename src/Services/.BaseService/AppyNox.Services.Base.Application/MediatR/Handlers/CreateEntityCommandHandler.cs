using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR.Handlers
{
    public sealed class CreateEntityCommandHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork)
        : BaseHandler<TEntity>(repository, mapper, dtoMappingRegistry, serviceProvider, logger, unitOfWork),
        IRequestHandler<CreateEntityCommand<TEntity>, (Guid guid, object basicDto)>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Public Methods ]

        public async Task<(Guid guid, object basicDto)> Handle(CreateEntityCommand<TEntity> request, CancellationToken cancellationToken)
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
                await Repository.AddAsync(mappedEntity);
                await UnitOfWork.SaveChangesAsync();

                Type returnDtoType = DtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.DataAccess, entityType, CommonDtoLevelEnums.Simple.GetDisplayName());
                object createdObject = Mapper.Map(mappedEntity, returnDtoType, returnDtoType);
                return (guid: mappedEntity.Id, basicDto: createdObject);
            }
            catch (Exception ex) when (ex is INoxInfrastructureException || ex is INoxApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new NoxApplicationException(ex);
            }
        }

        #endregion
    }
}