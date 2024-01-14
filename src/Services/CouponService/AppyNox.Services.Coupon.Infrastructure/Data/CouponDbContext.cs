using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Coupon.Infrastructure.Data
{
    public class CouponDbContext : DbContext
    {
        #region [ Public Constructors ]

        public CouponDbContext()
        {
        }

        public CouponDbContext(DbContextOptions options)
            : base(options)
        {
        }

        #endregion

        #region [ Properties ]

        public DbSet<CouponEntity> Coupons { get; set; }

        public DbSet<CouponDetailEntity> CouponDetails { get; set; }

        #endregion

        #region [ Protected Methods ]

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ Common Ids ]

            Guid couponDetailId = Guid.Parse("ec80532f-58f0-4690-b40c-2133b067d5f2");
            Guid couponId1 = Guid.Parse("594cf045-3a2b-46f5-99c9-1eb59f035db2");
            Guid couponId2 = Guid.Parse("c386aec2-dfd2-4ea5-b878-8fe5632e2e40");

            #endregion

            #region [ Entity Configurations ]

            modelBuilder.ApplyConfiguration(new CouponConfiguration(couponId1, couponId2, couponDetailId));
            modelBuilder.ApplyConfiguration(new CouponDetailConfiguration(couponDetailId));

            #endregion
        }

        #endregion
    }
}