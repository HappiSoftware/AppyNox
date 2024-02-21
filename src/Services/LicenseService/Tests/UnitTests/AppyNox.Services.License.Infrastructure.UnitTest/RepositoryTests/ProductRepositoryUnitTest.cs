using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.UnitTest.Seeds.ProductSeeds;
using Moq;

namespace AppyNox.Services.License.Infrastructure.UnitTest.RepositoryTests
{
    public class ProductRepositoryUnitTest(RepositoryFixture fixture) : IClassFixture<RepositoryFixture>
    {
        #region [ Fields ]

        private readonly RepositoryFixture _fixture = fixture;

        private readonly NoxInfrastructureLoggerStub _noxLoggerStub = fixture.NoxLoggerStub;

        private readonly ICacheService _cacheService = fixture.RedisCacheService.Object;

        #endregion

        #region [ CRUD Methods ]

        #region [ Read ]

        [Fact]
        public async Task GetAllAsync_ShouldReturnEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            context.SeedOneProduct();
            ProductRepository repository = new(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 1,
            };
            PaginatedList result = await repository.GetAllAsync(queryParameters, typeof(ProductSimpleDto), _cacheService);

            Assert.NotNull(result);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnTwo()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            context.SeedMultipleProducts(2);
            ProductRepository repository = new(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 2,
            };

            PaginatedList result = await repository.GetAllAsync(queryParameters, typeof(ProductSimpleDto), _cacheService);

            Assert.NotNull(result);
            Assert.Equal(2, result.ItemsCount);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnCorrectEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            var products = context.SeedMultipleProducts(2);
            Assert.NotNull(products);

            ProductRepository repository = new(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 2,
                PageSize = 1,
            };
            PaginatedList result = await repository.GetAllAsync(queryParameters, typeof(ProductSimpleUpdateDto), _cacheService);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(products.Last().Id.Value, ((ProductSimpleUpdateDto)result.Items.First()).Id.Value);
            Assert.Equal(products.Last().Name, ((ProductSimpleUpdateDto)result.Items.First()).Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFifty()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            context.SeedMultipleProducts(50);
            ProductRepository repository = new(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 50,
            };
            PaginatedList result = await repository.GetAllAsync(queryParameters, typeof(ProductSimpleUpdateDto), _cacheService);

            Assert.NotNull(result);
            Assert.Equal(50, result.ItemsCount);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            IEnumerable<ProductEntity> products = context.SeedMultipleProducts(50);
            Assert.NotNull(products);
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);
            ProductRepository repository = new(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 5,
                PageSize = 5,
            };
            PaginatedList result = await repository.GetAllAsync(queryParameters, typeof(ProductSimpleUpdateDto), _cacheService);

            List<ProductEntity> expectedProducts = products.Skip(20).Take(5).ToList();
            List<object> resultList = result.Items.ToList();

            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.ItemsCount);

            for (int i = 0; i < expectedProducts.Count; i++)
            {
                Assert.Equal(expectedProducts[i].Id.Value, ((ProductSimpleUpdateDto)resultList[i]).Id.Value);
                Assert.Equal(expectedProducts[i].Name, ((ProductSimpleUpdateDto)resultList[i]).Name);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            ProductEntity existingProducts = context.SeedOneProduct();
            Assert.NotNull(existingProducts);

            ProductRepository repository = new(context, _noxLoggerStub);
            ProductEntity result = await repository.GetByIdAsync(existingProducts.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldValuesBeCorrect()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            ProductEntity existingProducts = context.SeedOneProduct();
            Assert.NotNull(existingProducts);
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);

            ProductRepository repository = new(context, _noxLoggerStub);
            ProductEntity result = await repository.GetByIdAsync(existingProducts.Id);

            Assert.NotNull(result);
            Assert.Equal(existingProducts.Code, result.Code);
            Assert.Equal(existingProducts.Name, result.Name);
        }

        #endregion

        #region [ Create ]

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            var unitOfWork = new UnitOfWork(context, _noxLoggerStub);
            ProductRepository repository = new(context, _noxLoggerStub);

            ProductEntity product = ProductEntity.Create("NewProduct");

            await repository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            var asd = context.Licenses.ToList();
            var result = await repository.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Code, result.Code);
            Assert.Equal(product.Name, result.Name);
        }

        #endregion

        #region [ Update ]

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            var existingProduct = context.SeedOneProduct();
            Assert.NotNull(existingProduct);

            var unitOfWork = new UnitOfWork(context, _noxLoggerStub);
            ProductRepository repository = new(context, _noxLoggerStub);

            existingProduct.Name = "NewProductUp";
            existingProduct.Code = "ORP01";

            repository.Update(existingProduct);
            await unitOfWork.SaveChangesAsync();

            var result = await repository.GetByIdAsync(existingProduct.Id);

            Assert.NotNull(result);
            Assert.Equal(existingProduct.Id, result.Id);
            Assert.Equal(existingProduct.Code, result.Code);
            Assert.Equal(existingProduct.Name, result.Name);
        }

        #endregion

        #region [ Delete ]

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            var existingProduct = context.SeedOneProduct();
            Assert.NotNull(existingProduct);

            var unitOfWork = new UnitOfWork(context, _noxLoggerStub);
            ProductRepository repository = new(context, _noxLoggerStub);

            await repository.RemoveByIdAsync(existingProduct.Id);
            await unitOfWork.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<EntityNotFoundException<ProductEntity>>(async () =>
            {
                var result = await repository.GetByIdAsync(existingProduct.Id);
            });

            Assert.NotNull(exception);
        }

        #endregion

        #endregion
    }
}