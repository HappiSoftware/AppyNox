﻿using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AppyNox.Services.Base.Application.MediatR.Commands
{
    [SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
    public record CreateEntityCommand<TEntity>(dynamic Dto, string DetailLevel) : IRequest<(Guid guid, object basicDto)>;

    [SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
    public record DeleteEntityCommand<TEntity>(TEntity Entity) : IRequest where TEntity : class, IEntityTypeId;

    [SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
    public record UpdateEntityCommand<TEntity>(Guid Id, dynamic Dto, string DetailLevel) : IRequest where TEntity : class, IEntityTypeId;

    [SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
    public record GetAllEntitiesQuery<TEntity>(IQueryParameters QueryParameters) : IRequest<PaginatedList>;

    [SuppressMessage("Sonar Code Smell", "S2326:Unused type parameters should be removed", Justification = "TEntity is used to specify the type of entity being created")]
    public record GetEntityByIdQuery<TEntity>(Guid Id, IQueryParameters QueryParameters) : IRequest<object>;
}