using AppyNox.Services.Coupon.Application.DTOUtilities;
using AppyNox.Services.Coupon.Application.Services.Interfaces;
using AppyNox.Services.Coupon.Domain.Common;
using AppyNox.Services.Coupon.Domain.Interfaces;
using AppyNox.Services.Coupon.Infrastructure.ExceptionExtensions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Services.Implementations
{
    public class GenericService<TEntity, TDto, TCreateDTO, TUpdateDTO> : IGenericService<TEntity, TDto, TCreateDTO, TUpdateDTO>
    where TEntity : class, IEntityWithGuid
    where TDto : class
    where TCreateDTO : class
    where TUpdateDTO : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly DTOMappingRegistry _dtoMappingRegistry;
        private readonly Type _detailLevelsEnum;
        private readonly IUnitOfWork _unitOfWork;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper, DTOMappingRegistry dtoMappingRegistry, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _dtoMappingRegistry = dtoMappingRegistry;
            _detailLevelsEnum = _dtoMappingRegistry.GetDetailLevelType(typeof(TEntity));
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<dynamic>> GetAllAsync(QueryParameters queryParameters, string? detailLevel)
        {
            if (string.IsNullOrEmpty(detailLevel))
            {
                detailLevel = GetBasicDetailLevel();
            }

            var dtoType = _dtoMappingRegistry.GetDTOType(_detailLevelsEnum, typeof(TEntity), detailLevel);

            var entities = await _repository.GetAllAsync(queryParameters, dtoType);
            var result = _mapper.Map(entities, entities.GetType(), typeof(IEnumerable<>).MakeGenericType(dtoType));
            if (result is IEnumerable<dynamic> validResult)
            {
                return validResult;
            }
            return new List<dynamic>();
        }

        public async Task<dynamic?> GetByIdAsync(Guid id, string? detailLevel)
        {
            try
            {
                if (string.IsNullOrEmpty(detailLevel))
                {
                    detailLevel = GetBasicDetailLevel();
                }
                var dtoType = _dtoMappingRegistry.GetDTOType(_detailLevelsEnum, typeof(TEntity), detailLevel);
                var entity = await _repository.GetByIdAsync(id, dtoType);
                return _mapper.Map(entity, entity.GetType(), dtoType);
            }
            catch (EntityNotFoundException<TEntity>)
            {
                // Handle the specific case if needed when the entity is not found
                return null;
            }
            catch (Exception ex)
            {
                // (mustang) Log the error, return an appropriate response, or rethrow if needed
                throw;
            }
        }

        public async Task<(Guid guid, TDto basicDto)> AddAsync(TCreateDTO dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            var createdObject = _mapper.Map<TDto>(entity);
            return (entity.Id, createdObject);
        }

        public async Task UpdateAsync(TUpdateDTO dto)
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

        #region [ Private Methods ]

        private string GetBasicDetailLevel()
        {
            return "Basic";
        }

        #endregion
    }
}
