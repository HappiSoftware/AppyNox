using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AppyNox.Services.Coupon.Infrastructure.Data;
using CouponAggregate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

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

        internal static CouponAggregate SeedOneCoupon(this CouponDbContext context)
        {
            return SeedMultipleCoupons(context, 1, 1).First();
        }

        internal static IEnumerable<CouponAggregate> SeedMultipleCoupons(this CouponDbContext context, int couponSize, int couponDetailSize)
        {
            if (couponSize <= 0)
            {
                throw new ArgumentException("Coupon size must be greater than 0.", nameof(couponSize));
            }
            if (couponDetailSize <= 0)
            {
                throw new ArgumentException("CouponDetail size must be greater than 0.", nameof(couponDetailSize));
            }

            var couponDetails = new List<CouponDetail>();
            var coupons = new List<CouponAggregate>();
            Random random = new();

            #region [ CouponDetails ]

            var codeIdentifier = CodeIdentifier;

            for (int i = 0; i < couponDetailSize; i++)
            {
                CouponDetail couponDetail = new CouponDetailBuilder().WithDetails($"EXD{codeIdentifier}", $"DescriptionCouponDetail{codeIdentifier++}").Build();
                couponDetails.Add(couponDetail);
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
                Amount amount = new(discountAmount, minAmount);
                CouponAggregate couponEntity = new CouponBuilder().WithDetails($"EXF{codeIdentifier}", $"DescriptionCoupon{codeIdentifier++}", "test detail")
                                                                   .WithAmount(amount)
                                                                   .WithCouponDetailId(couponDetails[0].Id)
                                                                   .Build();

                couponEntity.AddAuditInformation("admin", DateTime.UtcNow);
                couponEntity.UpdateAuditInformation("admin", DateTime.UtcNow);
                minAmount++;
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