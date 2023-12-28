using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds.CouponSeeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest
{
    public class InfrastructureTestBase : IDisposable
    {
        #region [ Fields ]

        protected readonly DbContextOptions<CouponDbContext> _options;

        protected readonly CouponDbContext _context;

        protected readonly NoxInfrastructureLoggerStub _noxLoggerStub = new();

        #endregion

        #region [ Events ]

        protected static event Func<CouponDbContext, CouponEntity>? SeedOneCoupon;

        protected static event Func<CouponDbContext, int, int, IEnumerable<CouponEntity>>? SeedMultipleCoupons;

        #endregion

        #region [ Public Constructors ]

        protected InfrastructureTestBase()
        {
            _options = new DbContextOptionsBuilder<CouponDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new CouponDbContext(_options);

            SeedOneCoupon += GenericCouponTestSeedData.SeedOneCoupon;
            SeedMultipleCoupons += GenericCouponTestSeedData.SeedMultipleCoupons;
        }

        #endregion

        #region [ Public Methods ]

        public void Dispose()
        {
            SeedOneCoupon -= GenericCouponTestSeedData.SeedOneCoupon;
            SeedMultipleCoupons -= GenericCouponTestSeedData.SeedMultipleCoupons;
            _context.Database.EnsureDeleted();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region [ Protected Methods ]

        protected static CouponEntity? RaiseSeedOneCoupon(CouponDbContext context)
        {
            return SeedOneCoupon?.Invoke(context);
        }

        protected static IEnumerable<CouponEntity>? RaiseSeedMultipleCoupons(CouponDbContext context, int couponSize, int couponDetailSize)
        {
            return SeedMultipleCoupons?.Invoke(context, couponSize, couponDetailSize);
        }

        #endregion
    }
}