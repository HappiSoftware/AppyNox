using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

internal class UpdateEntityCommandHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider, logger),
        IRequestHandler<UpdateEntityCommand<TEntity>>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly IGenericRepositoryBase<TEntity> _repository = repository;

    private readonly IUnitOfWorkBase _unitOfWork = unitOfWork;

    #endregion

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
            _repository.Update(mappedEntity, propertyList);
            await _unitOfWork.SaveChangesAsync(NoxContext.UserId.ToString());
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error updating entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
            throw new NoxApplicationException(ex, (int)NoxApplicationExceptionCode.GenericUpdateCommandError);
        }
    }

    #endregion
}