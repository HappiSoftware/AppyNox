using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Domain.Attributes;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Domain.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace AppyNox.Services.Base.Infrastructure.Data;

public abstract class NoxDatabaseContext(DbContextOptions options, IEncryptionService? encryptionService = null)
    : DbContext(options), INoxDatabaseContext
{
    private readonly IEncryptionService? _encryptionService = encryptionService;

    #region [ Properties ]

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    #endregion

    #region [ Protected Constructors ]

    #endregion

    #region [ Protected Methods ]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureAuditableEntities(modelBuilder);
        ConfigureOutboxMessages(modelBuilder);
        ApplyEncryptionConverters(modelBuilder);

    }

    private static void ConfigureAuditableEntities(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                var entity = modelBuilder.Entity(entityType.ClrType);
                entity.Property<string>("CreatedBy").IsRequired(true);
                entity.Property<DateTime>("CreationDate").IsRequired(true);
                entity.Property<string?>("UpdatedBy").IsRequired(false);
                entity.Property<DateTime?>("UpdateDate").IsRequired(false);
                entity.HasIndex("CreationDate");
                entity.HasIndex("UpdateDate");
            }
        }
    }

    private static void ConfigureOutboxMessages(ModelBuilder modelBuilder)
    {
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

    private void ApplyEncryptionConverters(ModelBuilder modelBuilder)
    {
        if (_encryptionService == null)
        {
            return;
        }
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var propertyInfo = property.PropertyInfo;
                if (propertyInfo == null) continue;

                if (propertyInfo.GetCustomAttribute<EncryptAttribute>() != null)
                {
                    var converter = new ValueConverter<string, string>(
                        v => _encryptionService.EncryptString(v),
                        v => _encryptionService.DecryptString(v));

                    property.SetValueConverter(converter);
                }
            }
        }
    }

    #endregion
}