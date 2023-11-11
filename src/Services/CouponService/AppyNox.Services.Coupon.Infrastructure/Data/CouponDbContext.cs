using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

            ConfigureCoupon(modelBuilder);
        }

        #endregion

        #region [ Private Methods ]

        private void ConfigureCoupon(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CouponConfiguration());

            modelBuilder.Entity<CouponEntity>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CouponEntity>().HasData(
                new CouponEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "EXF50",
                    Description = "Description",
                    DiscountAmount = 10.65,
                    MinAmount = 100
                });

            modelBuilder.Entity<CouponEntity>().HasData(
                new CouponEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "EXF60",
                    Description = "Description2",
                    DiscountAmount = 20.55,
                    MinAmount = 200
                });
        }

        #endregion
    }
}