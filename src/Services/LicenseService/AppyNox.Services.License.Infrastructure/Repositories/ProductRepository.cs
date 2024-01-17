using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.License.Application.Interfaces;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;

namespace AppyNox.Services.License.Infrastructure.Repositories
{
    public class ProductRepository(LicenseDatabaseContext context, INoxInfrastructureLogger noxInfrastructureLogger)
        : GenericRepositoryBase<ProductEntity>(context, noxInfrastructureLogger), IProductInterface
    {
    }
}