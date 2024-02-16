using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds.CouponSeeds
{
    internal static class GenericCouponTestSeedData
    {
        #region [ Properties ]

        private static double DiscountAmount { get; set; } = 10.0;

        private static int MinAmount { get; set; } = 100;

        private static int CodeIdentifier { get; set; } = 10;

        #endregion

        #region [ Protected Methods ]

        internal static CouponEntity SeedOneCoupon(this CouponDbContext context)
        {
            return SeedMultipleCoupons(context, 1, 1).First();
        }

        internal static IEnumerable<CouponEntity> SeedMultipleCoupons(this CouponDbContext context, int couponSize, int couponDetailSize)
        {
            if (couponSize <= 0)
            {
                throw new ArgumentException("Coupon size must be greater than 0.", nameof(couponSize));
            }
            if (couponDetailSize <= 0)
            {
                throw new ArgumentException("CouponDetail size must be greater than 0.", nameof(couponDetailSize));
            }

            var couponDetails = new List<CouponDetailEntity>();
            var coupons = new List<CouponEntity>();
            var random = new Random();

            #region [ CouponDetails ]

            var codeIdentifier = CodeIdentifier;

            for (int i = 0; i < couponDetailSize; i++)
            {
                var couponEntity = new CouponDetailEntity
                {
                    Id = Guid.NewGuid(),
                    Code = $"EXD{codeIdentifier}",
                    Detail = $"DescriptionCouponDetail{codeIdentifier++}"
                }; ;
                couponDetails.Add(couponEntity);
            }
            context.CouponDetails.AddRange(couponDetails);
            context.SaveChanges();

            #endregion

            #region [ Coupons ]

            var discountAmount = DiscountAmount;
            var minAmount = MinAmount;
            codeIdentifier = CodeIdentifier;

            for (int i = 0; i < couponSize; i++)
            {
                var couponEntity = new CouponEntity
                {
                    Id = new CouponId(Guid.NewGuid()),
                    Code = $"EXF{codeIdentifier}",
                    Description = $"DescriptionCoupon{codeIdentifier++}",
                    DiscountAmount = discountAmount,
                    MinAmount = minAmount++,
                    CouponDetailEntityId = couponDetails[random.Next(couponDetails.Count)].Id
                };
                discountAmount += 0.1;
                coupons.Add(couponEntity);
            }
            context.Coupons.AddRange(coupons);
            context.SaveChanges();

            #endregion

            return coupons;
        }

        #endregion
    }
}