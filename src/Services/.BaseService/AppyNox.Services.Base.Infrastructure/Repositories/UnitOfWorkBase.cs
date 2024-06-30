using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppyNox.Services.Base.Infrastructure.Repositories;

/// <summary>
/// Provides an implementation of the Unit of Work pattern.
/// This class manages transactions and changes to the database context in a cohesive manner.
/// </summary>
public abstract class UnitOfWorkBase(DbContext dbContext, INoxInfrastructureLogger logger) : IUnitOfWork
{
    #region [ Fields ]

    private readonly DbContext _dbContext = dbContext;

    private readonly INoxInfrastructureLogger _logger = logger;

    private IDbContextTransaction? _transaction;

    private bool _disposed = false;

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Finalizes an instance of the <see cref="UnitOfWorkBase"/> class.
    /// </summary>
    ~UnitOfWorkBase()
    {
        Dispose(false);
    }

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    public void BeginTransaction()
    {
        _logger.LogInformation("Beginning a new database transaction.");
        _transaction = _dbContext.Database.BeginTransaction();
    }

    /// <summary>
    /// Commits the current transaction, saving all changes to the database.
    /// </summary>
    /// <exception cref="Exception">Thrown if an error occurs during the commit process.</exception>
    public void Commit()
    {
        try
        {
            _logger.LogInformation("Attempting to commit the current transaction.");
            _transaction!.Commit();
            _logger.LogInformation("Transaction committed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while committing the  transaction.");
            Rollback();
            throw new CommitException(ex);
        }
    }

    /// <summary>
    /// Rolls back the current transaction, reverting all changes made within the transaction scope.
    /// </summary>
    public void Rollback()
    {
        _logger.LogWarning("Attempting to rollback the current transaction.");
        _transaction!.Rollback();
    }

    /// <summary>
    /// Saves all changes made in the context of the database asynchronously.
    /// </summary>
    /// <returns>The number of objects written to the underlying database.</returns>
    public async Task<int> SaveChangesAsync(bool isSystem = false)
    {
        string userId = "Unknown";
        if (NoxContext.UserId != Guid.Empty)
        {
            userId = NoxContext.UserId.ToString();
        }
        if (isSystem)
        {
            userId = "System";
        }

        foreach (var entry in _dbContext.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreationDate").CurrentValue = DateTime.UtcNow;
                entry.Property("CreatedBy").CurrentValue = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedBy").IsModified = false;
                entry.Property("CreationDate").IsModified = false;
                entry.Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                entry.Property("UpdatedBy").CurrentValue = userId;
            }
        }

        foreach (var entry in _dbContext.ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("IsDeleted").CurrentValue = false;
                entry.Property("DeletedDate").CurrentValue = null;
                entry.Property("DeletedBy").CurrentValue = null;
            }
            else if (entry.State == EntityState.Modified && entry.Property("IsDeleted").CurrentValue is true)
            {
                entry.Property("DeletedDate").CurrentValue = DateTime.UtcNow;
                entry.Property("DeletedBy").CurrentValue = userId;
            }
        }

        _logger.LogInformation($"Attempting to saving changes to the database asynchronously, responsible user: {userId}.");

        return await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="UnitOfWorkBase"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="UnitOfWorkBase"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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