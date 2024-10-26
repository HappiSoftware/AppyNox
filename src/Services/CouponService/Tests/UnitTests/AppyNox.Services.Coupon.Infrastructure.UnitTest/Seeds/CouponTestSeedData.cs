﻿using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using CouponAggregate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds;

internal static class CouponTestSeedData
{
    #region [ Properties ]

    private static double DiscountAmount { get; set; } = 10.0;

    private static int MinAmount { get; set; } = 100;

    private static int CodeIdentifier { get; set; } = 10;

    #endregion

    #region [ Internal Methods ]

    internal static async Task<CouponAggregate> SeedOneCoupon(this CouponDbContext context, UnitOfWork unitOfWork)
    {
        return (await context.SeedMultipleCoupons(unitOfWork, 1, 1)).First();
    }

    internal static async Task<IEnumerable<CouponAggregate>> SeedMultipleCoupons(this CouponDbContext context, UnitOfWork unitOfWork, int couponSize, int couponDetailSize)
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

        #region [ CouponDetails ]

        var codeIdentifier = CodeIdentifier;

        for (int i = 0; i < couponDetailSize; i++)
        {
            CouponDetail couponDetail = new CouponDetailBuilder().WithDetails($"EXD{codeIdentifier}", $"DescriptionCouponDetail{codeIdentifier++}").Build();
            couponDetails.Add(couponDetail);
        }
        context.CouponDetails.AddRange(couponDetails);
        await unitOfWork.SaveChangesAsync();

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

            minAmount++;
            discountAmount += 0.1;
            coupons.Add(couponEntity);
        }
        context.Coupons.AddRange(coupons);
        await unitOfWork.SaveChangesAsync();

        #endregion

        return coupons;
    }

    #endregion
}