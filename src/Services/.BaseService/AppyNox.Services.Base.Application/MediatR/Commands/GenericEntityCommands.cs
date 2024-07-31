using AppyNox.Services.Base.Application.Interfaces.MediatR;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AppyNox.Services.Base.Application.MediatR.Commands;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record CreateEntityCommand<TEntity>(dynamic Dto, string DetailLevel, NoxCommandExtensions Extensions = default!) 
    : IRequest<Guid>, IHaveNoxCommandExtensions
    where TEntity : IEntityWithGuid;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record DeleteEntityCommand<TEntity>(Guid Id, bool ForceDelete = false, NoxCommandExtensions Extensions = default!) 
    : IRequest, IHaveNoxCommandExtensions
    where TEntity : IEntityWithGuid;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record UpdateEntityCommand<TEntity>(Guid Id, dynamic Dto, string DetailLevel, NoxCommandExtensions Extensions = default!) 
    : IRequest, IHaveNoxCommandExtensions
    where TEntity : IEntityWithGuid;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record GetAllEntitiesQuery<TEntity>(IQueryParameters QueryParameters, NoxCommandExtensions Extensions = default!) 
    : IRequest<PaginatedList<object>>, IHaveNoxCommandExtensions
    where TEntity : IEntityWithGuid;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record GetEntityByIdQuery<TEntity>(Guid Id, IQueryParameters QueryParameters, bool Track = false, NoxCommandExtensions Extensions = default!) 
    : IRequest<object>, IHaveNoxCommandExtensions
    where TEntity : IEntityWithGuid;