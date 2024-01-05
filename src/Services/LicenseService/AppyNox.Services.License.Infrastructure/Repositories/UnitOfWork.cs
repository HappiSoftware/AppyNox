using AppyNox.Services.Base.Infrastructure.Logger;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.Data;

namespace AppyNox.Services.License.Infrastructure.Repositories
{
    public class UnitOfWork(LicenseDatabaseContext dbContext, INoxInfrastructureLogger logger)
        : UnitOfWorkBase(dbContext, logger)
    {
    }
}