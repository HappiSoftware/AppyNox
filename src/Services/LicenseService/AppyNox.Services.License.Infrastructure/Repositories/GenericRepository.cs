using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.Data;

namespace AppyNox.Services.License.Infrastructure.Repositories;

public class GenericRepository<TEntity>(
        LicenseDatabaseContext context,
        INoxInfrastructureLogger<GenericRepositoryBase<TEntity>> noxInfrastructureLogger)
        : GenericRepositoryBase<TEntity>(context, noxInfrastructureLogger) where TEntity : class, IEntityWithGuid
{
}