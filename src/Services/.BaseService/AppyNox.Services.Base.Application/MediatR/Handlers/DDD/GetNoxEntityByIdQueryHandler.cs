using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AutoMapper;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Handlers.DDD;

internal class GetNoxEntityByIdQueryHandler<TEntity, TId, TDto>(
        INoxRepository<TEntity> repository,
        IMapper mapper,
        IServiceProvider serviceProvider,
        INoxApplicationLogger<GetNoxEntityByIdQueryHandler<TEntity, TId, TDto>> logger)
        : BaseHandler<TEntity>(mapper, serviceProvider),
        IRequestHandler<GetNoxEntityByIdQuery<TEntity, TId, TDto>, TDto>
        where TEntity : class, IHasStronglyTypedId
        where TId : NoxId
{
    #region [ Fields ]

    private readonly INoxRepository<TEntity> _repository = repository;

    private readonly IMapper _mapper = mapper;

    #endregion

    #region [ Public Methods ]

    public async Task<TDto> Handle(GetNoxEntityByIdQuery<TEntity, TId, TDto> request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {request.Id}.");
            TEntity entity = await _repository.GetByIdAsync(request.Id, request.QueryParameters.IncludeDeleted, request.Track);
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