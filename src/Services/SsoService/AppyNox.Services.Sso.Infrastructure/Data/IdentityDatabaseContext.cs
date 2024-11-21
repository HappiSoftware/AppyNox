using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Domain.Attributes;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Domain.Outbox;
using AppyNox.Services.Base.Infrastructure.Data;
using AppyNox.Services.Sso.Application.AsyncLocals;
using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Sso.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Reflection.Emit;

namespace AppyNox.Services.Sso.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for identity-related entities, extending the IdentityDbContext with custom configurations.
    /// </summary>
    public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options, IEncryptionService? encryptionService = null)
    : IdentityDbContext<
                        ApplicationUser,            // User entity
                        ApplicationRole,            // Role entity
                        Guid,                       // Key type
                        IdentityUserClaim<Guid>,    // User claims
                        IdentityUserRole<Guid>,     // User-Role join entity
                        IdentityUserLogin<Guid>,    // User logins
                        IdentityRoleClaim<Guid>,    // Role claims
                        IdentityUserToken<Guid>     // User tokens
                        >(options), INoxDatabaseContext
    {

        private readonly IEncryptionService? _encryptionService = encryptionService;

        #region [ Properties ]

        public DbSet<Company> Companies { get; set; }
        public override DbSet<ApplicationUser> Users { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        #endregion

        #region [ Protected Methods ]

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region [ Common Ids ]

            Guid adminUserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");
            Guid superAdminUserId = Guid.Parse("6e54d3e3-90d0-4604-91b4-77009cedd760");
            Guid notAdminUserId = Guid.Parse("2c0e21cf-4845-4b0f-a653-6b7c414af2f9");

            Guid adminRoleId = Guid.Parse("e24e99e7-00e4-4007-a042-565eac12d96d");
            Guid superAdminRoleId = Guid.Parse("f51a5d58-ff38-4563-9d32-f658ef2b40d0");
            Guid notAdminRoleId = Guid.Parse("4d0f77eb-2ad5-4b43-848e-826cd32d684b");

            Guid companyId = Guid.Parse("221e8b2c-59d5-4e5b-b010-86c239b66738");
            Guid happiCompanyId = Guid.Parse("0ebae1bf-6610-4967-a8ed-b149219caf68");

            #endregion

            #region [ Entity Configurations ]

            builder.ApplyConfiguration(new CompanyConfiguration(happiCompanyId, companyId));
            builder.ApplyConfiguration(new ApplicationRoleConfiguration(adminRoleId, companyId, superAdminRoleId, notAdminRoleId, happiCompanyId));
            builder.ApplyConfiguration(new ApplicationUserConfiguration(adminUserId, companyId, superAdminUserId, notAdminUserId, happiCompanyId));
            builder.ApplyConfiguration(new ApplicationRoleClaimConfiguration(adminRoleId, notAdminRoleId));
            builder.ApplyConfiguration(new ApplicationUserRoleConfiguration(adminRoleId, adminUserId, superAdminRoleId, superAdminUserId, notAdminRoleId, notAdminUserId));

            #endregion

            #region [ GlobalQueries ]

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasQueryFilter(u =>
                    IsConnectRequest() || IsSuperAdmin() ||
                    IsAdmin() && u.CompanyId == GetCurrentCompanyId() && u.Id != GetCurrentUserId());
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.HasQueryFilter(r =>
                    IsConnectRequest() || IsSuperAdmin() ||
                    IsAdmin() && r.CompanyId == GetCurrentCompanyId());
            });

            #endregion

            #region [ INoxDatabaseContext ]

            ConfigureEntities(builder);
            ConfigureOutboxMessages(builder);
            ApplyEncryptionConverters(builder);

            #endregion
        }

        #endregion

        #region [ Global Query Helper Methods ]

        private bool IsSuperAdmin()
        {
            return SsoContext.IsSuperAdmin;
        }

        private bool IsAdmin()
        {
            return SsoContext.IsAdmin;
        }

        private Guid GetCurrentCompanyId()
        {
            return SsoContext.CompanyId;
        }

        private Guid GetCurrentUserId()
        {
            // TODO inspect here
            //return NoxContext.UserId; // I dont remember why this line was added. Turned off this filtering for now
            return Guid.Empty;
        }

        private bool IsConnectRequest()
        {
            return SsoContext.IsConnectRequest;
        }

        #endregion

        #region [ Protected Methods ]

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

    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDatabaseContext>
    {
        public IdentityDatabaseContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<IdentityDatabaseContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new IdentityDatabaseContext(builder.Options);
        }
    }
}