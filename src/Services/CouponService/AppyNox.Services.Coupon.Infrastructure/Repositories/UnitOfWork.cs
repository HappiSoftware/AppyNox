﻿using AppyNox.Services.Coupon.Domain.Interfaces;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region [ Fields ]

        private readonly CouponDbContext _dbContext;

        private IDbContextTransaction? _transaction;

        private bool _disposed = false;

        #endregion

        #region [ Public Constructors ]

        public UnitOfWork(CouponDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region [ Public Methods ]

        ~UnitOfWork()
        {
            Dispose(false);
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                }

                _disposed = true;
            }
        }

        #endregion
    }
}