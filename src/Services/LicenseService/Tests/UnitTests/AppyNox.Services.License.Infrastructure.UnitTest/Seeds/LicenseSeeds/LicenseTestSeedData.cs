using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;

namespace AppyNox.Services.License.Infrastructure.UnitTest.Seeds.LicenseSeeds;

internal static class LicenseTestSeedData
{
    #region [ Properties ]

    private static int CodeIdentifier { get; set; } = 1;

    #endregion

    #region [ Internal Methods ]

    internal static async Task<LicenseEntity> SeedOneLicense(this LicenseDatabaseContext context, UnitOfWork unitOfWork)
    {
        return (await SeedMultipleLicenses(context, unitOfWork, 1)).First();
    }

    internal static async Task<IEnumerable<LicenseEntity>> SeedMultipleLicenses(this LicenseDatabaseContext context, UnitOfWork unitOfWork, int licenseSize)
    {
        if (licenseSize <= 0)
        {
            throw new ArgumentException("License size must be greater than 0.", nameof(licenseSize));
        }

        var licenses = new List<LicenseEntity>();

        #region [ Licenses ]

        int codeIdentifier = CodeIdentifier;

        for (int i = 0; i < licenseSize; i++)
        {
            LicenseEntity license = LicenseEntity.Create
            (
                $"LK{codeIdentifier:D3}",
                $"DescriptionCoupon{codeIdentifier++}",
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddDays(10),
                1,
                2,
                Guid.NewGuid(),
                new ProductId(Guid.NewGuid())
            );
            licenses.Add(license);
        }
        context.Licenses.AddRange(licenses);
        await unitOfWork.SaveChangesAsync();

        #endregion

        return licenses;
    }

    #endregion
}