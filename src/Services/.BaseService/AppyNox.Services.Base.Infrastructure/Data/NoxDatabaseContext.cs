using AppyNox.Services.Base.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Base.Infrastructure.Data;

public abstract class NoxDatabaseContext : DbContext
{
    #region [ Protected Constructors ]

    protected NoxDatabaseContext()
        : base()
    {
    }

    protected NoxDatabaseContext(DbContextOptions options)
        : base(options)
    {
    }

    #endregion

    #region [ Protected Methods ]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure shadow properties for entities marked with IAuditable
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                var entity = modelBuilder.Entity(entityType.ClrType);

                // Defining shadow properties
                entity.Property<string>("CreatedBy").IsRequired(true);
                entity.Property<DateTime>("CreationDate").IsRequired(true);
                entity.Property<string?>("UpdatedBy").IsRequired(false);
                entity.Property<DateTime?>("UpdateDate").IsRequired(false);

                entity.HasIndex("CreationDate");
                entity.HasIndex("UpdateDate");
            }
        }
    }

    #endregion
}