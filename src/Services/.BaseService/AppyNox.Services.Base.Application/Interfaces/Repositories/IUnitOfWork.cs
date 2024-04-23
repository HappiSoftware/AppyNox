namespace AppyNox.Services.Base.Application.Interfaces.Repositories;

/// <summary>
/// Defines a contract for a unit of work, which groups one or more operations into a logical transaction.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    #region [ Public Methods ]

    /// <summary>
    /// Begins a transaction.
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    void Rollback();

    /// <summary>
    /// Saves all changes made in the context of the unit of work.
    /// </summary>
    /// <returns>The number of objects written to the underlying database.</returns>
    Task<int> SaveChangesAsync(bool isSystem = false);

    #endregion
}