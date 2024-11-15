using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;

internal class GetEntityByIdQueryHandler<TEntity, TDto>(
        IGenericRepository<TEntity> repository,
        IMapper mapper,
        IServiceProvider serviceProvider,
        INoxApplicationLogger<GetEntityByIdQueryHandler<TEntity, TDto>> logger)
        : BaseHandler<TEntity>(mapper, serviceProvider),
        IRequestHandler<GetEntityByIdQuery<TEntity, TDto>, TDto>
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    private readonly IGenericRepository<TEntity> _repository = repository;

    private readonly IMapper _mapper = mapper;

    #endregion

    #region [ Public Methods ]

    public async Task<TDto> Handle(GetEntityByIdQuery<TEntity, TDto> request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {request.Id}.");
            var entity = await _repository.GetByIdAsync(request.Id, request.QueryParameters.IncludeDeleted, request.Track);
            return _mapper.Map<TDto>(entity);
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