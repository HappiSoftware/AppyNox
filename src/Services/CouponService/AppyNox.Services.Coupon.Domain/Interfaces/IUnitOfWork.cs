namespace AppyNox.Services.Coupon.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region [ Public Methods ]

        void BeginTransaction();

        void Commit();

        void Rollback();

        Task<int> SaveChangesAsync();

        #endregion
    }
}