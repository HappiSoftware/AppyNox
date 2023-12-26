using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AutoMapper;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        private readonly IServiceProvider _serviceProvider;

        private readonly Type _entityType;

        #endregion

        #region [ Public Constructors ]

        public GenericServiceBase(IGenericRepositoryBase<TEntity> repository, IMapper mapper, IDtoMappingRegistryBase dtoMappingRegistry,
            IUnitOfWorkBase unitOfWork, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _dtoMappingRegistry = dtoMappingRegistry;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _entityType = (this as IGenericServiceBase<TEntity>).GetEntityType();
        }

        #endregion

        #region [ Public Methods ]

        public async Task<IEnumerable<dynamic>> GetAllAsync(QueryParametersBase queryParameters)
        {
            var (expression, dtoType) = CreateProjection(queryParameters);
            var entities = await _repository.GetAllAsync(queryParameters, expression);
            List<object> resultList = [];

            if (dtoType != null)
            {
                foreach (var entity in entities)
                {
                    if (dtoType == typeof(ExpandoObject) && queryParameters.CommonDtoLevel == CommonDtoLevelEnums.IdOnly)
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
            }
            else
            {
                resultList.AddRange(entities);
            }

            return resultList;
        }

        public async Task<dynamic> GetByIdAsync(Guid id, QueryParametersBase queryParameters)
        {
            var (expression, dtoType) = CreateProjection(queryParameters);
            var entity = await _repository.GetByIdAsync(id, expression);
            dynamic result;

            if (dtoType == typeof(ExpandoObject) && queryParameters.CommonDtoLevel == CommonDtoLevelEnums.IdOnly)
            {
                // Create dynamic object with only Id property
                result = new ExpandoObject();
                result.Id = entity.GetType().GetProperty("Id")?.GetValue(entity);
                return result;
            }
            else
            {
                result = _mapper.Map(entity, entity.GetType(), dtoType);
            }
            return result;
        }

        public async Task<(Guid guid, dynamic basicDto)> AddAsync(dynamic dto, string detailLevel)
        {
            #region [ Dynamic Dto Convertion ]

            var dtoType = _dtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.Create, _entityType, detailLevel);
            var dtoObject = JsonSerializer.Deserialize(dto, dtoType, options: _jsonSerializerOptions);

            #endregion

            #region [ FluentValidation ]

            FluentValidate(dtoType, dtoObject);

            #endregion

            var mappedEntity = _mapper.Map(dtoObject, dtoType, _entityType);
            await _repository.AddAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            var returnDtoType = _dtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.DataAccess, _entityType, CommonDtoLevelEnums.Simple.GetDisplayName());
            var createdObject = _mapper.Map(mappedEntity, returnDtoType, returnDtoType);
            return (guid: mappedEntity.Id, basicDto: createdObject);
        }

        public async Task UpdateAsync(dynamic dto, string detailLevel)
        {
            #region [ Dynamic Dto Convertion ]

            var dtoType = _dtoMappingRegistry.GetDtoType(DtoLevelMappingTypes.Update, typeof(TEntity), detailLevel);
            var dtoObject = JsonSerializer.Deserialize(dto, dtoType, options: _jsonSerializerOptions);

            #endregion

            #region [ FluentValidation ]

            FluentValidate(dtoType, dtoObject);

            #endregion

            var mappedEntity = _mapper.Map(dtoObject, dtoType, typeof(TEntity));
            List<string> propertyList = (dtoObject as object).GetType().GetProperties().Select(x => x.Name).ToList();
            _repository.UpdateAsync(mappedEntity, propertyList);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var newEntity = Activator.CreateInstance<TEntity>();
            newEntity.Id = id;
            _repository.DeleteAsync(newEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region [ Protected Methods ]

        protected (Expression<Func<TEntity, dynamic>> expression, Type? dtoType) CreateProjection(QueryParametersBase queryParameters)
        {
            Type? dtoType = null;
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
            }

            return (_repository.CreateProjection(properties), dtoType);
        }

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

        #endregion
    }
}