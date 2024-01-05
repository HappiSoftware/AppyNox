using AppyNox.Services.Base.Infrastructure.Logger;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories
{
    public class UnitOfWork(CouponDbContext dbContext, INoxInfrastructureLogger logger)
        : UnitOfWorkBase(dbContext, logger)
    {
    }
}