namespace AppyNox.Services.Base.Infrastructure.Interfaces
{
    public interface IUnitOfWorkBase : IDisposable
    {
        #region [ Public Methods ]

        void BeginTransaction();

        void Commit();

        void Rollback();

        Task<int> SaveChangesAsync();

        #endregion
    }
}