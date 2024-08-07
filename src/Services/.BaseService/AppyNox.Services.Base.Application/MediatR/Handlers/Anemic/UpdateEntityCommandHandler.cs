using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System.Dynamic;
using System.Net;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

internal class UpdateEntityCommandHandler<TEntity>(
        IGenericRepository<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger<UpdateEntityCommandHandler<TEntity>> logger,
        IUnitOfWork unitOfWork)
        : BaseHandler<TEntity>(mapper, dtoMappingRegistry, serviceProvider),
        IRequestHandler<UpdateEntityCommand<TEntity>>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly IGenericRepository<TEntity> _repository = repository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #endregion

    #region [ Public Methods ]

    public async Task Handle(UpdateEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        try
        {
            #region [ Dynamic Dto Convertion ]

            Type dtoType = DtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.Update, typeof(TEntity), request.DetailLevel);
            dynamic dtoObject = JsonSerializer.Deserialize(request.Dto, dtoType, options: JsonSerializerOptions);

            #endregion

            #region [ Controls Before Update ]

            if (dtoObject is not IUpdateDto)
            {
                throw new NoxApplicationException(
                    NoxApplicationResourceService.IUpdateDtoNullId,
                    (int)NoxApplicationExceptionCode.IUpdateDtoNullId,
                    (int)HttpStatusCode.UnprocessableContent);
            }
            if (dtoObject is IUpdateDto updateDto && !updateDto.Id.Equals(request.Id))
            {
                throw new NoxApplicationException(
                    NoxApplicationResourceService.MismatchedIdInUpdate.Format(updateDto.Id, request.Id),
                    (int)NoxApplicationExceptionCode.MismatchedIdInUpdate,
                    (int)HttpStatusCode.UnprocessableContent);
            }
            dynamic idDto = new ExpandoObject();
            idDto.Id = request.Id;
            TEntity existingEntity = await _repository.GetByIdAsync(request.Id);

            #endregion

            logger.LogInformation($"Updating entity ID: '{request.Id}' type '{typeof(TEntity).Name}'");

            #region [ FluentValidation ]

            FluentValidate(dtoType, dtoObject);

            #endregion

            _repository.Update(existingEntity, dtoObject);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex) when (ex is INoxException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error updating entity of type '{typeof(TEntity).Name}' with ID: {request.Id}.");
            throw new NoxApplicationException(exceptionCode: (int)NoxApplicationExceptionCode.GenericUpdateCommandError, innerException: ex);
        }
    }

    #endregion
}