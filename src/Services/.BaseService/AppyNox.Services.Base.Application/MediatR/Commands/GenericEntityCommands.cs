using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Commands
{
    public record CreateEntityCommand<TEntity>(dynamic Dto, string DetailLevel) : IRequest<(Guid guid, object basicDto)>;
    public record DeleteEntityCommand<TEntity>(Guid Id) : IRequest where TEntity : class, IEntityWithGuid;
    public record UpdateEntityCommand<TEntity>(Guid Id, dynamic Dto, string DetailLevel) : IRequest where TEntity : class, IEntityWithGuid;

    public record GetAllEntitiesQuery<TEntity>(IQueryParameters QueryParameters) : IRequest<IEnumerable<object>>;
    public record GetEntityByIdQuery<TEntity>(Guid Id, IQueryParameters QueryParameters) : IRequest<object>;
}