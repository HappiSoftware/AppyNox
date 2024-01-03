﻿using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Logger;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories
{
    public class GenericRepository<TEntity>(CouponDbContext context, INoxInfrastructureLogger noxInfrastructureLogger)
        : GenericRepositoryBase<TEntity>(context, noxInfrastructureLogger) where TEntity : class, IEntityWithGuid
    {
    }
}