using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Infrastructure.Repositories
{
    /// <summary>
    /// Defines a generic repository abstract class for CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity the repository manages.</typeparam>
    public abstract class GenericRepositoryBase<TEntity> : IGenericRepositoryBase<TEntity> where TEntity : class, IEntityWithGuid
    {
        #region [ Fields ]

        private readonly DbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        private readonly INoxInfrastructureLogger _logger;

        #endregion

        #region [ Protected Constructors ]

        protected GenericRepositoryBase(DbContext context, INoxInfrastructureLogger logger)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _logger = logger;
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Retrieves an entity of type TEntity by its ID, selecting specific columns based on the provided expression.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <param name="selectedColumns">An expression defining the columns to select for the entity.</param>
        /// <returns>The entity of type TEntity</returns>
        /// <exception cref="EntityNotFoundException{TEntity}">Thrown when TEntity is not found by given ID</exception>
        /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
        public async Task<TEntity> GetByIdAsync(Guid id, Expression<Func<TEntity, dynamic>> selectedColumns)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
                var entity = await _dbSet.Where(item => item.Id == id).Select(selectedColumns).FirstOrDefaultAsync();

                if (entity == null)
                {
                    _logger.LogWarning($"Entity with ID: {id} not found.");
                    throw new EntityNotFoundException<TEntity>(id);
                }

                _logger.LogInformation($"Successfully retrieved entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
                return entity;
            }
            catch (NoxInfrastructureException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving entity with ID: '{id}' Type: '{typeof(TEntity).Name}'.");
                throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.DataFetchingError);
            }
        }

        public async Task<IEnumerable<object>> GetAllAsync(IQueryParameters queryParameters, Expression<Func<TEntity, dynamic>> selectedColumns)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve entities. Type: '{typeof(TEntity).Name}'.");
                var entities = await _dbSet
                    .AsQueryable()
                    .Select(selectedColumns)
                    .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .ToListAsync();

                _logger.LogInformation($"Successfully retrieved entities. Type: '{typeof(TEntity).Name}'.");
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving entities. Type: '{typeof(TEntity).Name}'.");
                throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.MultipleDataFetchingError);
            }
        }

        /// <summary>
        /// Adds a new entity of type TEntity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity of type TEntity</returns>
        /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                _logger.LogInformation($"Attempting to add a new entity. Type: '{typeof(TEntity).Name}'.");
                await _context.Set<TEntity>().AddAsync(entity);
                _logger.LogInformation($"Successfully added a new entity. Type: '{typeof(TEntity).Name}'.");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding a new entity. Type: '{typeof(TEntity).Name}'.");
                throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.AddingDataError);
            }
        }

        /// <summary>
        /// Updates an existing entity of type TEntity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="properties">A list of property names to update.</param>
        /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
        public void Update(TEntity entity, IList<string> properties)
        {
            try
            {
                _logger.LogInformation($"Attempting to update entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
                _context.Set<TEntity>().Entry(entity).State = EntityState.Unchanged;
                properties = properties.Where(p => p != nameof(entity.Id)).ToList();
                foreach (var property in properties)
                {
                    _context.Entry(entity).Property(property).IsModified = true;
                }
                _logger.LogInformation($"Successfully updated entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
                throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.UpdatingDataError);
            }
        }

        /// <summary>
        /// Removes an entity of type TEntity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
        public void Remove(TEntity entity)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
                _dbSet.Remove(entity);
                _logger.LogInformation($"Successfully deleted entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting entity with ID: '{entity.Id}' Type: '{typeof(TEntity).Name}'.");
                throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.DeletingDataError);
            }
        }

        #endregion

        #region [ Public Helper Methods ]

        /// <summary>
        /// Creates a projection expression for an entity of type TEntity based on a list of property names.
        /// </summary>
        /// <param name="propertyNames">A list of property names to include in the projection.</param>
        /// <returns>An expression used for selecting specific properties of the entity.</returns>
        /// <exception cref="NoxInfrastructureException">Thrown if an unexpected error occurs.</exception>
        public Expression<Func<TEntity, dynamic>> CreateProjection(List<string> propertyNames)
        {
            try
            {
                _logger.LogInformation($"Creating projection for entity properties. Type: '{typeof(TEntity).Name}'.");

                var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
                var memberBindings = new List<MemberBinding>();

                foreach (var propertyName in propertyNames)
                {
                    var entityProperty = Expression.Property(entityParameter, propertyName);
                    var conversion = Expression.Convert(entityProperty, entityProperty.Type);
                    var memberBinding = Expression.Bind(entityProperty.Member, conversion);
                    memberBindings.Add(memberBinding);
                }

                var memberInit = Expression.MemberInit(Expression.New(typeof(TEntity)), memberBindings);
                var selector = Expression.Lambda<Func<TEntity, dynamic>>(memberInit, entityParameter);

                _logger.LogInformation($"Successfully created projection for entity properties. Type: '{typeof(TEntity).Name}'.");
                return selector;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating projection for entity properties. Type: '{typeof(TEntity).Name}'.");
                throw new NoxInfrastructureException(ex, (int)NoxInfrastructureExceptionCode.ProjectionError);
            }
        }

        #endregion
    }
}