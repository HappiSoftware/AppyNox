using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Services.Implementations
{
    public class GenericService<TEntity, TDto, TCreateDto, TUpdateDto> : GenericServiceBase<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntityWithGuid
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
    {
        #region Public Constructors

        public GenericService(IGenericRepositoryBase<TEntity> repository, IMapper mapper, DtoMappingRegistry dtoMappingRegistry, IUnitOfWorkBase unitOfWork)
            : base(repository, mapper, dtoMappingRegistry, unitOfWork)
        {
        }

        #endregion
    }
}