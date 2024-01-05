using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.MediatR.Handlers;
using AppyNox.Services.Base.Application.MediatR.Queries;
using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Base.Application.Helpers
{
    public static class ServiceCollectionExtensions
    {
        #region [ Public Methods ]

        /// <summary>
        /// Registers MediatR command handlers specific to a given entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type for which to register command handlers.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> after registration.</returns>
        /// <remarks>
        /// This method registers handlers for various entity-related operations like
        /// getting all entities, getting an entity by ID, creating, updating, and deleting an entity.
        /// </remarks>
        public static IServiceCollection AddEntityCommandHandlers<TEntity>(this IServiceCollection services)
        where TEntity : class, IEntityWithGuid
        {
            // Register GetAllEntitiesQueryHandler
            services.AddTransient<IRequestHandler<GetAllEntitiesQuery<TEntity>, IEnumerable<object>>,
                GetAllEntitiesQueryHandler<TEntity>>();

            // Register GetEntityByIdQueryHandler
            services.AddTransient<IRequestHandler<GetEntityByIdQuery<TEntity>, object>,
                GetEntityByIdQueryHandler<TEntity>>();

            // Register CreateEntityCommandHandler
            services.AddTransient<IRequestHandler<CreateEntityCommand<TEntity>, (Guid id, object dto)>,
                CreateEntityCommandHandler<TEntity>>();

            // Register UpdateEntityCommandHandler
            services.AddTransient<IRequestHandler<UpdateEntityCommand<TEntity>>,
                UpdateEntityCommandHandler<TEntity>>();

            // Register DeleteEntityCommandHandler
            services.AddTransient<IRequestHandler<DeleteEntityCommand<TEntity>>,
                DeleteEntityCommandHandler<TEntity>>();

            return services;
        }

        /// <summary>
        /// Registers MediatR command handlers for multiple entity types.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="entityTypes">An array of entity types for which to register command handlers.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> after registration.</returns>
        /// <remarks>
        /// This method dynamically registers command handlers for a variety of entity types.
        /// It leverages <see cref="AddEntityCommandHandlers{TEntity}"/> for each specified entity type.
        /// </remarks>
        public static IServiceCollection AddGenericEntityCommandHandlers(this IServiceCollection services, params Type[] entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var addMethod = typeof(ServiceCollectionExtensions)
                    .GetMethod(nameof(AddEntityCommandHandlers), BindingFlags.Static | BindingFlags.Public)
                    ?.MakeGenericMethod(entityType);

                addMethod?.Invoke(null, [services]);
            }

            return services;
        }

        #endregion
    }
}