using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories
{
    public class GenericRepository<TEntity>(CouponDbContext context, INoxInfrastructureLogger noxInfrastructureLogger)
        : GenericRepositoryBase<TEntity>(context, noxInfrastructureLogger) where TEntity : class, IEntityWithGuid
    {
    }
}