using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;

namespace AppyNox.Services.Coupon.Infrastructure.Data
{
    public class CouponDbContext : DbContext
    {
        #region [ Public Constructors ]

        public CouponDbContext()
        {
        }

        public CouponDbContext(DbContextOptions<CouponDbContext> options) : base(options)
        {
        }

        #endregion

        #region [ Properties ]

        public DbSet<CouponEntity> Coupons { get; set; }

        public DbSet<CouponDetailEntity> CouponDetails { get; set; }

        #endregion

        #region [ Protected Methods ]

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Get environment
                string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Console.WriteLine($"Environment: {environment}");

                // Build config
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AppyNox.Services.Coupon.WebAPI"))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
                var connectionString = config.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Connection String: {connectionString}");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureCouponDetail(modelBuilder);
            ConfigureCoupon(modelBuilder);
        }

        #endregion

        #region [ Private Methods ]

        private static void ConfigureCoupon(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CouponConfiguration());

            builder.Entity<CouponEntity>()
                .HasKey(c => c.Id);

            builder.Entity<CouponEntity>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<CouponEntity>()
                .HasOne(c => c.CouponDetailEntity)
                .WithMany(cd => cd.Coupons)
                .HasForeignKey(c => c.CouponDetailEntityId)
                .IsRequired();

            CouponDetailEntity couponDetailEntity =
                new()
                {
                    Id = Guid.Parse("c2feaca4-d82a-4d2e-ba5a-667b685212b4"),
                    Code = "EXF50",
                    Detail = "TestDetail"
                };

            builder.Entity<CouponDetailEntity>().HasData(couponDetailEntity);

            builder.Entity<CouponEntity>().HasData(
                new CouponEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "EXF50",
                    Description = "Description",
                    DiscountAmount = 10.65,
                    MinAmount = 100,
                    Detail = "Detail1",
                    CouponDetailEntityId = Guid.Parse("c2feaca4-d82a-4d2e-ba5a-667b685212b4")
                });

            builder.Entity<CouponEntity>().HasData(
                new CouponEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "EXF60",
                    Description = "Description2",
                    DiscountAmount = 20.55,
                    MinAmount = 200,
                    Detail = "Detail2",
                    CouponDetailEntityId = Guid.Parse("c2feaca4-d82a-4d2e-ba5a-667b685212b4")
                });
        }

        private static void ConfigureCouponDetail(ModelBuilder builder)
        {
            builder.Entity<CouponDetailEntity>()
            .HasKey(cd => cd.Id);

            builder.Entity<CouponDetailEntity>()
                .Property(cd => cd.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<CouponDetailEntity>()
                .HasMany(cd => cd.Coupons)
                .WithOne(c => c.CouponDetailEntity)
                .HasForeignKey(c => c.CouponDetailEntityId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }

        #endregion
    }
}