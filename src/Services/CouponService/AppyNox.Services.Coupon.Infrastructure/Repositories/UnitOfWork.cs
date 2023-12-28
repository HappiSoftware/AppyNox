using AppyNox.Services.Base.Infrastructure.Logger;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories
{
    internal class UnitOfWork(CouponDbContext dbContext, INoxInfrastructureLogger logger)
        : UnitOfWorkBase(dbContext, logger)
    {
    }
}