using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Domain.Outbox;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Base.Infrastructure.Data;

public abstract class NoxDatabaseContext : DbContext
{
    #region [ Properties ]

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    #endregion

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

        // Outbox message
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(om => om.Type).IsRequired();
            entity.Property(om => om.Content).IsRequired();
            entity.Property(om => om.OccurredOnUtc).IsRequired();
            entity.Property(om => om.ProcessedOnUtc).IsRequired(false);
            entity.Property(om => om.Error).IsRequired(false);
            entity.Property(om => om.RetryCount).IsRequired();
        });
    }

    #endregion
}