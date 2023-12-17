using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AutoMapper;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Authentication.Application.Services.Interfaces;

namespace AppyNox.Services.Authentication.Application.Services.Implementations
{
    public class GenericService<TEntity> : GenericServiceBase<TEntity>, IGenericService<TEntity>
    where TEntity : class, IEntityWithGuid
    {
        #region [ Public Constructors ]

        public GenericService(IGenericRepositoryBase<TEntity> repository, IMapper mapper, IDtoMappingRegistryBase dtoMappingRegistry, IUnitOfWorkBase unitOfWork,
            IServiceProvider serviceProvider)
            : base(repository, mapper, dtoMappingRegistry, unitOfWork, serviceProvider)
        {
        }

        #endregion
    }
}