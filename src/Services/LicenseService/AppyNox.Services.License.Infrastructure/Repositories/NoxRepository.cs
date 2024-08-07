using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.Data;

namespace AppyNox.Services.License.Infrastructure.Repositories;

public class NoxRepository<TEntity>(
    LicenseDatabaseContext context,
    INoxInfrastructureLogger<NoxRepositoryBase<TEntity>> noxInfrastructureLogger)
        : NoxRepositoryBase<TEntity>(context, noxInfrastructureLogger) where TEntity : class, IHasStronglyTypedId
{
}