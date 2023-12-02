using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds.CouponSeeds
{
    internal class GenericCouponTestSeedData
    {
        #region Properties

        private static double _discountAmount { get; set; } = 10.0;

        private static int _minAmount { get; set; } = 100;

        private static int _codeIdentifier { get; set; } = 10;

        #endregion

        #region Internal Methods

        internal static CouponEntity SeedOneCoupon(CouponDbContext context)
        {
            return CreateCoupons(context, 1, 1).First();
        }

        internal static IEnumerable<CouponEntity> SeedMultipleCoupons(CouponDbContext context, int couponSize, int couponDetailSize)
        {
            return CreateCoupons(context, couponSize, couponDetailSize);
        }

        #endregion

        #region Protected Methods

        protected static IEnumerable<CouponEntity> CreateCoupons(CouponDbContext context, int couponSize, int couponDetailSize)
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

            var codeIdentifier = _codeIdentifier;

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

            var discountAmount = _discountAmount;
            var minAmount = _minAmount;
            codeIdentifier = _codeIdentifier;

            for (int i = 0; i < couponSize; i++)
            {
                var couponEntity = new CouponEntity
                {
                    Id = Guid.NewGuid(),
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

        #region Public Methods

        public static CouponEntity CreateCoupon()
        {
            var couponDetail = new CouponDetailEntity
            {
                Id = Guid.NewGuid(),
                Code = $"EXE10",
                Detail = $"DescriptionCouponDetail"
            };

            return new CouponEntity
            {
                Id = Guid.NewGuid(),
                Code = $"EXT10",
                Description = $"DescriptionCoupon",
                DiscountAmount = 10.5,
                MinAmount = 100,
                CouponDetailEntityId = couponDetail.Id
            };
        }

        #endregion
    }
}