using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;

namespace AppyNox.Services.License.Infrastructure.UnitTest.Seeds.ProductSeeds
{
    internal static class ProductTestSeedData
    {
        #region [ Properties ]

        private static int codeIdentifier { get; set; } = 1;

        #endregion

        #region [ Internal Methods ]

        internal static ProductEntity SeedOneProduct(this LicenseDatabaseContext context)
        {
            return SeedMultipleProducts(context, 1).First();
        }

        internal static IEnumerable<ProductEntity> SeedMultipleProducts(this LicenseDatabaseContext context, int productSize)
        {
            if (productSize <= 0)
            {
                throw new ArgumentException("Product size must be greater than 0.", nameof(productSize));
            }

            var products = new List<ProductEntity>();

            #region [ Licenses ]

            for (int i = 0; i < productSize; i++)
            {
                ProductEntity productEntity = new()
                {
                    Id = Guid.NewGuid(),
                    Code = $"PRO{codeIdentifier:D3}",
                    Name = $"ProductName{codeIdentifier++}",
                };
                products.Add(productEntity);
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            #endregion

            return products;
        }

        #endregion
    }
}