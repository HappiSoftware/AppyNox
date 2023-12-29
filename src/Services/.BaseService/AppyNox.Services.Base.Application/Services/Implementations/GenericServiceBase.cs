using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Application.Logger;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AutoMapper;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.Services.Implementations
{
    public class GenericServiceBase<TEntity> : IGenericServiceBase<TEntity>
    where TEntity : class, IEntityWithGuid
    {
        #region [ Fields ]

        private readonly IGenericRepositoryBase<TEntity> _repository;

        private readonly IMapper _mapper;

        private readonly IDtoMappingRegistryBase _dtoMappingRegistry;

        private readonly IUnitOfWorkBase _unitOfWork;

        private readonly IServiceProvider _serviceProvider;

        private readonly INoxApplicationLogger _logger;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        private readonly Type _entityType;

        #endregion

        #region [ Public Constructors ]

        public GenericServiceBase(IGenericRepositoryBase<TEntity> repository, IMapper mapper, IDtoMappingRegistryBase dtoMappingRegistry,
            IUnitOfWorkBase unitOfWork, IServiceProvider serviceProvider, INoxApplicationLogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _dtoMappingRegistry = dtoMappingRegistry;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _entityType = (this as IGenericServiceBase<TEntity>).GetEntityType();
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Retrieves entities of type TEntity based on the provided query parameters
        /// </summary>
        /// <param name="queryParameters">The query parameters for filtering and projection</param>
        /// <returns>A collection of dynamic objects representing the fetched entities</returns>
        /// <exception cref="NoxApplicationException">Thrown if an unexpected error occurs.</exception>
        public async Task<IEnumerable<dynamic>> GetAllAsync(QueryParametersBase queryParameters)
        {
            try
            {
                _logger.LogInformation($"Fetching entities of type '{typeof(TEntity).Name}'");
                var (expression, dtoType) = CreateProjection(queryParameters);
                IEnumerable<object> entities = await _repository.GetAllAsync(queryParameters, expression);
                List<object> resultList = MapEntitiesToDto(entities, dtoType, queryParameters);
                return resultList;
            }
            catch (NoxInfrastructureException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching entities of type '{typeof(TEntity).Name}'");
                throw new NoxApplicationException(ex);
            }
        }

        /// <summary>
        /// Retrieves a single entity of type TEntity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <param name="queryParameters">The query parameters for projection</param>
        /// <returns>A dynamic object representing the fetched entity.</returns>
        /// <exception cref="NoxApplicationException">Thrown if an unexpected error occurs.</exception>
        public async Task<dynamic> GetByIdAsync(Guid id, QueryParametersBase queryParameters)
        {
            try
            {
                _logger.LogInformation($"Fetching entity of type {typeof(TEntity).Name} with ID: {id}.");
                var (expression, dtoType) = CreateProjection(queryParameters);
                TEntity entity = await _repository.GetByIdAsync(id, expression);
                return MapEntityToDtoSingle(entity, dtoType);
            }
            catch (NoxInfrastructureException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching entity with ID: {id}.");
                throw new NoxApplicationException(ex);
            }
        }

        /// <summary>
        /// Add a new entity of type TEntity
        /// </summary>
        /// <param name="dto">The DTO object containing data for the new entity.</param>
        /// <param name="detailLevel">The detail level for mapping DTO to entity.</param>
        /// <returns>The ID of the newly added entity and the DTO object representing the entity.</returns>
        /// <exception cref="NoxApplicationException">Thrown if an unexpected error occurs.</exception>
        public async Task<(Guid guid, dynamic basicDto)> AddAsync(dynamic dto, string detailLevel)
        {
            try
            {
                _logger.LogInformation($"Adding new entity of type '{typeof(TEntity).Name}'");

                #region [ Dynamic Dto Convertion ]

                Type dtoType = _dtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.Create, _entityType, detailLevel);
                dynamic? dtoObject = JsonSerializer.Deserialize(dto, dtoType, options: _jsonSerializerOptions);

                #endregion

                #region [ FluentValidation ]

                FluentValidate(dtoType, dtoObject);

                #endregion

                TEntity mappedEntity = _mapper.Map(dtoObject, dtoType, _entityType);
                await _repository.AddAsync(mappedEntity);
                await _unitOfWork.SaveChangesAsync();

                Type returnDtoType = _dtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.DataAccess, _entityType, CommonDtoLevelEnums.Simple.GetDisplayName());
                dynamic createdObject = _mapper.Map(mappedEntity, returnDtoType, returnDtoType);
                return (guid: mappedEntity.Id, basicDto: createdObject);
            }
            catch (NoxInfrastructureException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new NoxApplicationException(ex);
            }
        }

        /// <summary>
        /// Updates an existing entity of type TEntity
        /// </summary>
        /// <param name="dto">The DTO object containing updated data for the entity.</param>
        ///<param name="detailLevel">The detail level for mapping DTO to entity.</param>
        /// <returns>Task</returns>
        /// <exception cref="NoxApplicationException">Thrown if an unexpected error occurs.</exception>
        public async Task UpdateAsync(dynamic dto, string detailLevel)
        {
            Guid? id = null;
            try
            {
                #region [ Dynamic Dto Convertion ]

                Type dtoType = _dtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.Update, typeof(TEntity), detailLevel);
                dynamic dtoObject = JsonSerializer.Deserialize(dto, dtoType, options: _jsonSerializerOptions);

                _logger.LogInformation($"Updating entity ID: '{dtoObject.Id}' type '{typeof(TEntity).Name}'");

                #endregion

                #region [ FluentValidation ]

                FluentValidate(dtoType, dtoObject);

                #endregion

                TEntity mappedEntity = _mapper.Map(dtoObject, dtoType, typeof(TEntity));
                List<string> propertyList = (dtoObject as object).GetType().GetProperties().Select(x => x.Name).ToList();
                _repository.Update(mappedEntity, propertyList);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating entity of type '{typeof(TEntity).Name}' with ID: {id}.");
                throw new NoxApplicationException(ex);
            }
        }

        /// <summary>
        /// Deletes an entity of type TEntity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>Task</returns>
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Deleting entity of type '{typeof(TEntity).Name}' with ID: {id}.");
                TEntity newEntity = Activator.CreateInstance<TEntity>();
                newEntity.Id = id;
                _repository.Remove(newEntity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting entity of type '{typeof(TEntity).Name}' with ID: {id}.");
                throw new NoxApplicationException(ex);
            }
        }

        #endregion

        #region [ Protected Methods ]

        /// <summary>
        /// Creates a projection expression based on tje provided query parameters
        /// </summary>
        /// <param name="queryParameters">The query parameters to determine the projection</param>
        /// <returns>A tuple conatining the projection expression and the DTO type</returns>
        /// <exception cref="DtoDetailLevelNotFoundException">Thrown if queryParameters.CommonDtoLevel is not fully covered in switch statements.
        /// It's not an expected case.</exception>
        protected (Expression<Func<TEntity, dynamic>> expression, Type dtoType) CreateProjection(QueryParametersBase queryParameters)
        {
            Type dtoType;
            List<string> properties = [];

            switch (queryParameters.CommonDtoLevel)
            {
                case CommonDtoLevelEnums.None:
                    dtoType = _dtoMappingRegistry.GetDtoType(queryParameters.AccessType, _entityType, queryParameters.DetailLevel);
                    properties = dtoType.GetProperties().Select(p => p.Name).ToList();
                    break;

                case CommonDtoLevelEnums.Simple:
                    dtoType = _dtoMappingRegistry.GetDtoType(queryParameters.AccessType, _entityType, CommonDtoLevelEnums.Simple.GetDisplayName());
                    properties = dtoType.GetProperties().Select(p => p.Name).ToList();
                    break;

                case CommonDtoLevelEnums.IdOnly:
                    properties.Add("Id");
                    dtoType = typeof(ExpandoObject);
                    break;

                default:
                    throw new DtoDetailLevelNotFoundException(queryParameters.CommonDtoLevel);
            }

            return (_repository.CreateProjection(properties), dtoType);
        }

        /// <summary>
        /// Performs validation on a given DTO using FluentValidation
        /// </summary>
        /// <param name="dtoType">The type of the DTO to validate</param>
        /// <param name="dtoObject">The DTO object to validate</param>
        /// <exception cref="ValidatorNotFoundException">Thrown if validator for the DTO is not found in Dependency Injection Container</exception>
        /// <exception cref="FluentValidationException">Thrown if DTO validation is not succeed</exception>
        protected void FluentValidate(Type dtoType, dynamic dtoObject)
        {
            Type genericType = typeof(IValidator<>).MakeGenericType(dtoType);
            IValidator validator = _serviceProvider.GetService(genericType) as IValidator ?? throw new ValidatorNotFoundException(dtoType);
            var context = new ValidationContext<object>(dtoObject, new PropertyChain(), new DefaultValidatorSelector());
            var validationResult = validator.Validate(context);
            if (!validationResult.IsValid)
            {
                throw new FluentValidationException(dtoType, validationResult);
            }
        }

        /// <summary>
        /// Maps a collection en entities to a specified DTO type
        /// </summary>
        /// <param name="entities">The entities to map</param>
        /// <param name="dtoType">The DTO type to map to</param>
        /// <param name="queryParametersBase">The query parameters for mapping logic</param>
        /// <returns>A list of mapped objects</returns>
        /// <exception cref="NoxApplicationException">Thrown if an unexpected error occurs.</exception>
        protected List<object> MapEntitiesToDto(IEnumerable<dynamic> entities, Type dtoType, QueryParametersBase queryParametersBase)
        {
            try
            {
                _logger.LogInformation($"Mapping entities of type '{typeof(TEntity).Name}' to '{dtoType.Name}' DTOs");
                List<object> resultList = [];
                foreach (var entity in entities)
                {
                    if (dtoType == typeof(ExpandoObject) && queryParametersBase.CommonDtoLevel == CommonDtoLevelEnums.IdOnly)
                    {
                        // Create dynamic object with only Id property
                        var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
                        dynamicObject["Id"] = entity.GetType().GetProperty("Id")!.GetValue(entity)!;
                        resultList.Add(dynamicObject);
                    }
                    else
                    {
                        var mappedEntity = _mapper.Map(entity, entity.GetType(), dtoType);
                        resultList.Add(mappedEntity);
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while mapping entities of type '{typeof(TEntity).Name}' to '{dtoType.Name}' DTOs");
                throw new NoxApplicationException(ex);
            }
        }

        /// <summary>
        /// Maps an entity to a specified DTO type
        /// </summary>
        /// <param name="entity">The entity to map</param>
        /// <param name="dtoType">The DTO type to map to</param>
        /// <returns>A mapped object</returns>
        /// <exception cref="NoxApplicationException">Thrown if an unexpected error occurs.</exception>
        protected object MapEntityToDtoSingle(dynamic entity, Type dtoType)
        {
            try
            {
                _logger.LogInformation($"Mapping single entity of type {typeof(TEntity).Name} to DTO.");
                object result;

                if (dtoType == typeof(ExpandoObject))
                {
                    var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
                    dynamicObject["Id"] = entity.GetType().GetProperty("Id")!.GetValue(entity)!;
                    result = dynamicObject;
                }
                else
                {
                    result = _mapper.Map(entity, entity.GetType(), dtoType);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while mapping single entity of type {typeof(TEntity).Name} to DTO.");
                throw new NoxApplicationException(ex);
            }
        }

        #endregion
    }
}