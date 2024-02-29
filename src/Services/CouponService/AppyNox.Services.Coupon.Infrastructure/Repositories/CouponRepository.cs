using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories;

public class CouponRepository<TEntity>(CouponDbContext context, INoxInfrastructureLogger noxInfrastructureLogger)
        : NoxRepositoryBase<TEntity>(context, noxInfrastructureLogger), ICouponRepository where TEntity : Domain.Coupons.Coupon, IHasStronglyTypedId
{
}

public interface ICouponRepository
{
}