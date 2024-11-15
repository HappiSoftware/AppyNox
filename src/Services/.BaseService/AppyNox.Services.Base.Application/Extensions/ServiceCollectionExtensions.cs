using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;
using AppyNox.Services.Base.Application.MediatR.Handlers.DDD;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Domain.DDD;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.Services.Base.Application.Extensions;

public static class ServiceCollectionExtensions
{
    #region [ Generic Service Methods ]

    /// <summary>
    /// Registers MediatR command handlers for Anemic Domain Entities.
    /// Used for Anemic Domain Modeling. If you are using Domain Driven Design, use
    /// <see cref="AddNoxEntityCommands"/> for entities with typed identifiers.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="services"></param>
    /// <returns>The updated <see cref="IServiceCollection"/> after registration.</returns>
    public static IServiceCollection AddAnemicEntityCommands<TEntity, TCreateDto, TDto, TUpdateDto>(this IServiceCollection services)
    where TEntity : class, IEntityWithGuid
    {
        // Register GetAllEntitiesQueryHandler
        services.AddTransient<IRequestHandler<GetAllEntitiesQuery<TEntity, TDto>, PaginatedList<TDto>>,
            GetAllEntitiesQueryHandler<TEntity, TDto>>();

        // Register GetEntityByIdQueryHandler
        services.AddTransient<IRequestHandler<GetEntityByIdQuery<TEntity, TDto>, TDto>,
            GetEntityByIdQueryHandler<TEntity, TDto>>();

        // Register CreateEntityCommandHandler
        services.AddTransient<IRequestHandler<CreateEntityCommand<TEntity, TCreateDto>, Guid>,
            CreateEntityCommandHandler<TEntity, TCreateDto>>();

        // Register UpdateEntityCommandHandler
        services.AddTransient<IRequestHandler<UpdateEntityCommand<TEntity, TUpdateDto>>,
            UpdateEntityCommandHandler<TEntity, TUpdateDto>>();

        // Register DeleteEntityCommandHandler
        services.AddTransient<IRequestHandler<DeleteEntityCommand<TEntity>>,
            DeleteEntityCommandHandler<TEntity>>();

        return services;
    }

    #endregion

    #region [ Nox Service Methods ]

    /// <summary>
    /// Registers MediatR command handlers for Domain Driven Design Entities.
    /// Used for Domain Driven Design Modeling. If you are usingAnemic Domain Modeling, use
    /// <see cref="AddAnemicEntityCommands"/> for entities with typed identifiers.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <param name="services"></param>
    /// <returns>The updated <see cref="IServiceCollection"/> after registration.</returns>
    public static IServiceCollection AddNoxEntityCommands<TEntity, TId, TCreateDto, TDto>(this IServiceCollection services)
        where TEntity : class, IHasStronglyTypedId
        where TId : NoxId
    {
        // Register GetAllNoxEntitiesQueryHandler
        services.AddTransient<IRequestHandler<GetAllNoxEntitiesQuery<TEntity, TDto>, PaginatedList<TDto>>,
            GetAllNoxEntitiesQueryHandler<TEntity, TDto>>();

        // Register GetNoxEntityByIdQueryHandler
        services.AddTransient<IRequestHandler<GetNoxEntityByIdQuery<TEntity, TId, TDto>, TDto>,
            GetNoxEntityByIdQueryHandler<TEntity, TId, TDto>>();

        // Register CreateNoxEntityCommandHandler
        services.AddTransient<IRequestHandler<CreateNoxEntityCommand<TEntity, TCreateDto>, Guid>,
            CreateNoxEntityCommandHandler<TEntity, TCreateDto>>();

        // Register DeleteNoxEntityCommandHandler
        services.AddTransient<IRequestHandler<DeleteNoxEntityCommand<TEntity, TId>>,
            DeleteNoxEntityCommandHandler<TEntity, TId>>();

        return services;
    }

    public static IServiceCollection AddNoxEntityCompositeCreateCommand<TEntity, TId, TCreateDto, TDto>(this IServiceCollection services)
    where TEntity : class, IHasStronglyTypedId
    where TId : NoxId
    {
        // Register CreateNoxEntityCommandHandler
        services.AddTransient<IRequestHandler<CreateNoxEntityCommand<TEntity, TCreateDto>, Guid>,
            CreateNoxEntityCommandHandler<TEntity, TCreateDto>>();

        return services;
    }

    #endregion
}