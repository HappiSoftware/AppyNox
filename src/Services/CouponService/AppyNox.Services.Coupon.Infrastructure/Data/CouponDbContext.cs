using AppyNox.Services.Coupon.Domain.Coupons;
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

        public DbSet<Domain.Coupons.Coupon> Coupons { get; set; }

        public DbSet<CouponDetail> CouponDetails { get; set; }

        public DbSet<CouponDetailTag> CouponDetailTags { get; set; }

        #endregion

        #region [ Protected Methods ]

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseNpgsql("User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ Common Ids ]

            Guid couponDetailId = Guid.Parse("ec80532f-58f0-4690-b40c-2133b067d5f2");
            Guid couponId1 = Guid.Parse("594cf045-3a2b-46f5-99c9-1eb59f035db2");
            Guid couponId2 = Guid.Parse("c386aec2-dfd2-4ea5-b878-8fe5632e2e40");
            Guid couponDetailTagId = Guid.Parse("b6bcfe76-83c7-4a4a-b088-13b14751fce8");

            #endregion

            #region [ Entity Configurations ]

            modelBuilder.ApplyConfiguration(new CouponConfiguration(couponId1, couponId2, couponDetailId));
            modelBuilder.ApplyConfiguration(new CouponDetailConfiguration(couponDetailId));
            modelBuilder.ApplyConfiguration(new CouponDetailTagConfiguration(couponDetailId, couponDetailTagId));

            #endregion
        }

        #endregion
    }
}