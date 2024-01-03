using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Logger;
using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.License.Application.Services.Interfaces;
using AutoMapper;

namespace AppyNox.Services.License.Application.Services.Implementations
{
    public class GenericService<TEntity> : GenericServiceBase<TEntity>, IGenericService<TEntity>
    where TEntity : class, IEntityWithGuid
    {
        #region [ Public Constructors ]

        public GenericService(IGenericRepositoryBase<TEntity> repository, IMapper mapper, IDtoMappingRegistryBase dtoMappingRegistry, IUnitOfWorkBase unitOfWork,
            IServiceProvider serviceProvider, INoxApplicationLogger logger)
            : base(repository, mapper, dtoMappingRegistry, unitOfWork, serviceProvider, logger)
        {
        }

        #endregion
    }
}