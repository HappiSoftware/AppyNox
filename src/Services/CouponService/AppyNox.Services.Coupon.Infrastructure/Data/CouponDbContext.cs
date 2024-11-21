using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Infrastructure.Data;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Coupon.Infrastructure.Data;

public class CouponDbContext(DbContextOptions<CouponDbContext> options, IEncryptionService? encryptionService = null)
        : NoxDatabaseContext(options, encryptionService)
{

    #region [ Public Constructors ]
    #endregion

    #region [ Properties ]

    public DbSet<Domain.Coupons.Coupon> Coupons { get; set; }

    public DbSet<CouponDetail> CouponDetails { get; set; }

    public DbSet<CouponDetailTag> CouponDetailTags { get; set; }

    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<TicketTag> TicketTags { get; set; }

    public DbSet<CouponHistory> CouponHistories { get; set; }

    #endregion

    #region [ Protected Methods ]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region [ Common Ids ]

        CouponDetailId couponDetailId = new(Guid.Parse("ec80532f-58f0-4690-b40c-2133b067d5f2"));
        CouponId couponId1 = new(Guid.Parse("594cf045-3a2b-46f5-99c9-1eb59f035db2"));
        CouponId couponId2 = new(Guid.Parse("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"));
        CouponDetailTagId couponDetailTagId = new(Guid.Parse("b6bcfe76-83c7-4a4a-b088-13b14751fce8"));
        Guid ticketId = new("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0");
        Guid ticketTagId = new("6125498b-ca83-4d9f-ae4d-55b97d98b47d");

        #endregion

        #region [ Entity Configurations ]

        // Domain Driven Design
        modelBuilder.ApplyConfiguration(new CouponConfiguration(couponId1, couponId2, couponDetailId));
        modelBuilder.ApplyConfiguration(new CouponDetailConfiguration(couponDetailId));
        modelBuilder.ApplyConfiguration(new CouponDetailTagConfiguration(couponDetailId, couponDetailTagId));

        // Anemic Domain Modeling
        modelBuilder.ApplyConfiguration(new TicketConfiguration(ticketId));
        modelBuilder.ApplyConfiguration(new TicketTagConfiguration(ticketTagId, ticketId));
        modelBuilder.ApplyConfiguration(new CouponHistoryConfiguration(couponId1));

        #endregion
    }

    #endregion

    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CouponDbContext>
    {
        public CouponDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<CouponDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new CouponDbContext(builder.Options);
        }
    }
}