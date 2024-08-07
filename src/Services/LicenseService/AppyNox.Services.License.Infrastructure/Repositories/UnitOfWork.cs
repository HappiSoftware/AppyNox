using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.Data;

namespace AppyNox.Services.License.Infrastructure.Repositories;

public class UnitOfWork(LicenseDatabaseContext dbContext, INoxInfrastructureLogger<UnitOfWorkBase> logger)
        : UnitOfWorkBase(dbContext, logger)
{
}