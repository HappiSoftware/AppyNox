using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AppyNox.Services.Base.Application.MediatR.Commands;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record GetAllNoxEntitiesQuery<TEntity>(IQueryParameters QueryParameters) : IRequest<PaginatedList<object>>
    where TEntity : class, IHasStronglyTypedId;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record GetNoxEntityByIdQuery<TEntity, TId>(TId Id, IQueryParameters QueryParameters, bool Track = false) : IRequest<object>
    where TEntity : class, IHasStronglyTypedId
    where TId : NoxId;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record CreateNoxEntityCommand<TEntity>(dynamic Dto, string DetailLevel) : IRequest<Guid>
    where TEntity : class, IHasStronglyTypedId;

[SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
public record DeleteNoxEntityCommand<TEntity, TId>(TId Id, bool ForceDelete = false) : IRequest
    where TEntity : class, IHasStronglyTypedId
    where TId : NoxId;