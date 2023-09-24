using CouponService.Domain.Interfaces;
using CouponService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CouponDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(CouponDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
                _transaction!.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transaction!.Rollback();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }

}
