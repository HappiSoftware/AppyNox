using AppyNox.Services.Base.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppyNox.Services.Base.Infrastructure.Repositories
{
    public class UnitOfWorkBase : IUnitOfWorkBase
    {
        #region [ Fields ]

        private readonly DbContext _dbContext;

        private IDbContextTransaction? _transaction;

        private bool _disposed = false;

        #endregion

        #region [ Public Constructors ]

        public UnitOfWorkBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region [ Public Methods ]

        ~UnitOfWorkBase()
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