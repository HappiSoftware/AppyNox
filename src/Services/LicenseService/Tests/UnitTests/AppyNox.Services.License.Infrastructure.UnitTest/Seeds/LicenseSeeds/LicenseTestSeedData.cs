﻿using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;

namespace AppyNox.Services.License.Infrastructure.UnitTest.Seeds.LicenseSeeds
{
    internal static class LicenseTestSeedData
    {
        #region [ Properties ]

        private static int CodeIdentifier { get; set; } = 1;

        #endregion

        #region [ Internal Methods ]

        internal static LicenseEntity SeedOneLicense(this LicenseDatabaseContext context)
        {
            return SeedMultipleLicenses(context, 1).First();
        }

        internal static IEnumerable<LicenseEntity> SeedMultipleLicenses(this LicenseDatabaseContext context, int licenseSize)
        {
            if (licenseSize <= 0)
            {
                throw new ArgumentException("Coupon size must be greater than 0.", nameof(licenseSize));
            }

            var licenses = new List<LicenseEntity>();
            var random = new Random();

            #region [ Licenses ]

            int codeIdentifier = CodeIdentifier;

            for (int i = 0; i < licenseSize; i++)
            {
                LicenseEntity licenseEntity = new()
                {
                    Id = Guid.NewGuid(),
                    Code = $"LK{codeIdentifier}",
                    Description = $"DescriptionCoupon{codeIdentifier++:D3}",
                    LicenseKey = Guid.NewGuid().ToString(),
                    ExpirationDate = DateTime.UtcNow.AddDays(10),
                    MaxUsers = 3
                };
                licenses.Add(licenseEntity);
            }
            context.Licenses.AddRange(licenses);
            context.SaveChanges();

            #endregion

            return licenses;
        }

        #endregion
    }
}