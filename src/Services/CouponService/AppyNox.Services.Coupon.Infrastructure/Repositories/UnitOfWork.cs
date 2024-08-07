using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories;

public class UnitOfWork(CouponDbContext dbContext, INoxInfrastructureLogger<UnitOfWorkBase> logger)
        : UnitOfWorkBase(dbContext, logger)
{
}