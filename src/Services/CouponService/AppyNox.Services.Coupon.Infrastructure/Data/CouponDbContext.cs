using AppyNox.Services.Base.Infrastructure.Services;
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

        public CouponDbContext(DbContextOptions<CouponDbContext> options)
            : base(options)
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

                // Build config
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AppyNox.Services.Coupon.WebAPI"))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
                var connectionString = config.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ Entity Configurations ]

            modelBuilder.ApplyConfiguration(new CouponConfiguration());
            modelBuilder.ApplyConfiguration(new CouponDetailConfiguration());

            #endregion
        }

        #endregion
    }
}