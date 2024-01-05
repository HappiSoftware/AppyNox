﻿using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Application.Logger;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AutoMapper;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR
{
    public abstract class BaseHandler<TEntity>(
        IGenericRepositoryBase<TEntity> repository,
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger,
        IUnitOfWorkBase unitOfWork)
        where TEntity : class, IEntityWithGuid
    {
        #region Fields

        protected readonly IGenericRepositoryBase<TEntity> Repository = repository;

        protected readonly IMapper Mapper = mapper;

        protected readonly IDtoMappingRegistryBase DtoMappingRegistry = dtoMappingRegistry;

        protected readonly IServiceProvider ServiceProvider = serviceProvider;

        protected readonly INoxApplicationLogger Logger = logger;

        protected readonly IUnitOfWorkBase UnitOfWork = unitOfWork;

        protected readonly Type EntityType = typeof(TEntity);

        protected readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

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
                    dtoType = DtoMappingRegistry.GetDtoType(queryParameters.AccessType, EntityType, queryParameters.DetailLevel);
                    properties = dtoType.GetProperties().Select(p => p.Name).ToList();
                    break;

                case CommonDtoLevelEnums.Simple:
                    dtoType = DtoMappingRegistry.GetDtoType(queryParameters.AccessType, EntityType, CommonDtoLevelEnums.Simple.GetDisplayName());
                    properties = dtoType.GetProperties().Select(p => p.Name).ToList();
                    break;

                case CommonDtoLevelEnums.IdOnly:
                    properties.Add("Id");
                    dtoType = typeof(ExpandoObject);
                    break;

                default:
                    throw new DtoDetailLevelNotFoundException(queryParameters.CommonDtoLevel);
            }

            return (Repository.CreateProjection(properties), dtoType);
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
            IValidator validator = ServiceProvider.GetService(genericType) as IValidator ?? throw new ValidatorNotFoundException(dtoType);
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
                Logger.LogInformation($"Mapping entities of type '{typeof(TEntity).Name}' to '{dtoType.Name}' DTOs");
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
                        var mappedEntity = Mapper.Map(entity, entity.GetType(), dtoType);
                        resultList.Add(mappedEntity);
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error occurred while mapping entities of type '{typeof(TEntity).Name}' to '{dtoType.Name}' DTOs");
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
                Logger.LogInformation($"Mapping single entity of type {typeof(TEntity).Name} to DTO.");
                object result;

                if (dtoType == typeof(ExpandoObject))
                {
                    var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
                    dynamicObject["Id"] = entity.GetType().GetProperty("Id")!.GetValue(entity)!;
                    result = dynamicObject;
                }
                else
                {
                    result = Mapper.Map(entity, entity.GetType(), dtoType);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error occurred while mapping single entity of type {typeof(TEntity).Name} to DTO.");
                throw new NoxApplicationException(ex);
            }
        }

        #endregion
    }
}