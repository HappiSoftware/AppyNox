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

    public bool IgnoreSoftDeleteFilter { get; set; } = false;
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    #endregion

    #region [ Protected Constructors ]

    #endregion

    #region [ Protected Methods ]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEntities(modelBuilder);
        ConfigureOutboxMessages(modelBuilder);
        ApplyEncryptionConverters(modelBuilder);

    }

    private void ConfigureEntities(ModelBuilder modelBuilder)
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

            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                var entity = modelBuilder.Entity(entityType.ClrType);
                entity.Property<bool>("IsDeleted").IsRequired().HasDefaultValue(false);
                entity.Property<DateTime?>("DeletedDate").IsRequired(false);
                entity.Property<string?>("DeletedBy").IsRequired(false);
                entity.HasIndex("IsDeleted");
                entity.HasIndex("DeletedDate");

                // Set the global filter
                var method = typeof(NoxDatabaseContext)
                    .GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static);
                if (method != null && method.IsGenericMethodDefinition)
                {
                    var genericMethod = method.MakeGenericMethod(entityType.ClrType);
                    genericMethod.Invoke(null, [modelBuilder, this]);
                }
                else
                {
                    // If method is not found
                    throw new InvalidOperationException($"Method {nameof(SetSoftDeleteFilter)} not found.");
                }
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

    private static void SetSoftDeleteFilter<T>(ModelBuilder modelBuilder, NoxDatabaseContext context) where T : class, ISoftDeletable
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => context.IgnoreSoftDeleteFilter || e.IsDeleted == false);
    }

    #endregion
}