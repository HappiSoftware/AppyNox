using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.DDD.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories;

public class NoxRepository<TEntity>(
    CouponDbContext context,
    INoxInfrastructureLogger<NoxRepositoryBase<TEntity>> noxInfrastructureLogger)
        : NoxRepositoryBase<TEntity>(context, noxInfrastructureLogger) where TEntity : class, IHasStronglyTypedId
{
}