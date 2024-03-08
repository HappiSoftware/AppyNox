using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;

namespace AppyNox.Services.License.Infrastructure.UnitTest.Seeds.ProductSeeds;

internal static class ProductTestSeedData
{
    #region [ Properties ]

    private static int CodeIdentifier { get; set; } = 1;

    #endregion

    #region [ Internal Methods ]

    internal static async Task<ProductEntity> SeedOneProduct(this LicenseDatabaseContext context, UnitOfWork unitOfWork)
    {
        return (await SeedMultipleProducts(context, unitOfWork, 1)).First();
    }

    internal static async Task<IEnumerable<ProductEntity>> SeedMultipleProducts(this LicenseDatabaseContext context, UnitOfWork unitOfWork, int productSize)
    {
        if (productSize <= 0)
        {
            throw new ArgumentException("Product size must be greater than 0.", nameof(productSize));
        }

        var products = new List<ProductEntity>();

        #region [ Licenses ]

        for (int i = 0; i < productSize; i++)
        {
            ProductEntity productEntity = ProductEntity.Create($"ProductName{CodeIdentifier++}");
            products.Add(productEntity);
        }
        context.Products.AddRange(products);
        await unitOfWork.SaveChangesAsync();

        #endregion

        return products;
    }

    #endregion
}