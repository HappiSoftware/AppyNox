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

        private readonly DtoMappingRegistryBase _dtoMappingRegistry;

        private readonly Dictionary<DtoLevelMappingTypes, Type> _detailLevelEnum;

        private readonly IUnitOfWorkBase _unitOfWork;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region [ Public Constructors ]

        public GenericServiceBase(IGenericRepositoryBase<TEntity> repository, IMapper mapper, DtoMappingRegistryBase dtoMappingRegistry,
            IUnitOfWorkBase unitOfWork, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _dtoMappingRegistry = dtoMappingRegistry;
            _detailLevelEnum = _dtoMappingRegistry.GetDetailLevelTypes(typeof(TEntity));
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
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

            return resultList.Count != 0 ? resultList : new List<dynamic>();
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

            var detailLevelMap = GetDetailLevelMap(DtoLevelMappingTypes.Create);
            var dtoType = _dtoMappingRegistry.GetDtoType(detailLevelMap, typeof(TEntity), detailLevel);
            var dtoObject = JsonSerializer.Deserialize(dto, dtoType, options: _jsonSerializerOptions);

            #endregion

            #region [ FluentValidation ]

            FluentValidate(dtoType, dtoObject);

            #endregion

            var mappedEntity = _mapper.Map(dtoObject, dtoType, typeof(TEntity));
            await _repository.AddAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            detailLevelMap = GetDetailLevelMap(DtoLevelMappingTypes.DataAccess);
            var returnDtoType = _dtoMappingRegistry.GetDtoType(detailLevelMap, typeof(TEntity), NoxEnumExtensions.GetDisplayName(CommonDtoLevelEnums.Simple));
            var createdObject = _mapper.Map(mappedEntity, returnDtoType, returnDtoType);
            return (guid: mappedEntity.Id, basicDto: createdObject);
        }

        public async Task UpdateAsync(dynamic dto)
        {
            #region [ Dynamic Dto Convertion ]

            var detailLevelMap = GetDetailLevelMap(DtoLevelMappingTypes.Update);
            var dtoType = _dtoMappingRegistry.GetDtoType(detailLevelMap, typeof(TEntity), NoxEnumExtensions.GetDisplayName(CommonDtoLevelEnums.Simple));

            #endregion

            #region [ FluentValidation ]

            FluentValidate(dtoType, dto);

            #endregion

            var mappedEntity = _mapper.Map(dto, dtoType, typeof(TEntity));
            _repository.UpdateAsync(mappedEntity);
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
            var detailLevelMap = GetDetailLevelMap(queryParameters.AccessType);

            switch (queryParameters.CommonDtoLevel)
            {
                case CommonDtoLevelEnums.None:
                    dtoType = _dtoMappingRegistry.GetDtoType(detailLevelMap, typeof(TEntity), queryParameters.DetailLevel);
                    properties = dtoType.GetProperties().Select(p => p.Name).ToList();
                    break;

                case CommonDtoLevelEnums.Simple:
                    dtoType = _dtoMappingRegistry.GetDtoType(detailLevelMap, typeof(TEntity), NoxEnumExtensions.GetDisplayName(CommonDtoLevelEnums.Simple));
                    properties = dtoType.GetProperties().Select(p => p.Name).ToList();
                    break;

                case CommonDtoLevelEnums.IdOnly:
                    properties.Add("Id");
                    dtoType = typeof(ExpandoObject);
                    break;
            }

            return (_repository.CreateProjection(properties), dtoType);
        }

        protected Type GetDetailLevelMap(DtoLevelMappingTypes dtoLevelMappingType)
        {
            return _detailLevelEnum.GetValueOrDefault(dtoLevelMappingType) ?? throw new AccessTypeNotFoundException(typeof(TEntity), dtoLevelMappingType.ToString());
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