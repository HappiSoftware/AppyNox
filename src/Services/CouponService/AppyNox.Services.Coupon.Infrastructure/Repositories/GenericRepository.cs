using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories;

internal class GenericRepository<TEntity>(CouponDbContext dbContext, INoxInfrastructureLogger logger)
    : GenericRepositoryBase<TEntity>(dbContext, logger) where TEntity : class, IEntityWithGuid
{
}