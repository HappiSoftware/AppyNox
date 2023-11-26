using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AutoMapper;
using System.Dynamic;
using System.Linq.Expressions;

namespace AppyNox.Services.Base.Application.Services.Implementations
{
    public class GenericServiceBase<TEntity, TDto, TCreateDto, TUpdateDto> : IGenericServiceBase<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntityWithGuid
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
    {
        #region [ Fields ]

        private readonly IGenericRepositoryBase<TEntity> _repository;

        private readonly IMapper _mapper;

        private readonly DtoMappingRegistryBase _dtoMappingRegistry;

        private readonly Dictionary<DtoLevelMappingTypes, Type> _detailLevelEnum;

        private readonly IUnitOfWorkBase _unitOfWork;

        #endregion

        #region [ Public Constructors ]

        public GenericServiceBase(IGenericRepositoryBase<TEntity> repository, IMapper mapper, DtoMappingRegistryBase dtoMappingRegistry, IUnitOfWorkBase unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _dtoMappingRegistry = dtoMappingRegistry;
            _detailLevelEnum = _dtoMappingRegistry.GetDetailLevelTypes(typeof(TEntity));
            _unitOfWork = unitOfWork;
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

        public async Task<dynamic?> GetByIdAsync(Guid id, QueryParametersBase queryParameters)
        {
            try
            {
                var (expression, dtoType) = CreateProjection(queryParameters);
                var entity = await _repository.GetByIdAsync(id, expression);
                object result;

                if (dtoType == typeof(ExpandoObject) && queryParameters.CommonDtoLevel == CommonDtoLevelEnums.IdOnly)
                {
                    // Create dynamic object with only Id property
                    result = new { Id = entity.GetType().GetProperty("Id")!.GetValue(entity) };
                }
                else
                {
                    result = _mapper.Map(entity, entity.GetType(), dtoType);
                }
                return result;
            }
            catch (EntityNotFoundException<TEntity>)
            {
                // Handle the specific case if needed when the entity is not found
                return null;
            }
            catch (Exception)
            {
                await Console.Out.WriteLineAsync("Error on GenericService:GetByIdAsync");

                // (mustang) Log the error, return an appropriate response, or rethrow if needed
                throw;
            }
        }

        public async Task<(Guid guid, TDto basicDto)> AddAsync(TCreateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            var createdObject = _mapper.Map<TDto>(entity);
            return (entity.Id, createdObject);
        }

        public async Task UpdateAsync(TUpdateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region [ Private Methods ]

        private (Expression<Func<TEntity, dynamic>> expression, Type? dtoType) CreateProjection(QueryParametersBase queryParameters)
        {
            Type? dtoType = null;
            List<string> properties = [];
            var detailLevelMap = _detailLevelEnum.GetValueOrDefault(queryParameters.AccessType) ?? throw new AccessTypeNotFoundException(typeof(TEntity), queryParameters.AccessType.ToString());

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

        #endregion
    }
}