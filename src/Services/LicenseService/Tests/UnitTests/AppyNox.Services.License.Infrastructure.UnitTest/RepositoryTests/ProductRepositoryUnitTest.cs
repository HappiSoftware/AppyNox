using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
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

        private readonly List<string> _propertyNames = typeof(ProductEntity)
            .GetProperties()
            .Select(p => p.Name)
            .Where(name => name != "Licenses")
            .ToList();

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
            var projection = repository.CreateProjection(_propertyNames);
            PaginatedList result = await repository.GetAllAsync(queryParameters, projection, _cacheService);

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

            var projection = repository.CreateProjection(_propertyNames);
            PaginatedList result = await repository.GetAllAsync(queryParameters, projection, _cacheService);

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
            var projection = repository.CreateProjection(_propertyNames);
            PaginatedList result = await repository.GetAllAsync(queryParameters, projection, _cacheService);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(products.Last().Id, ((ProductEntity)result.Items.First()).Id);
            Assert.Equal(products.Last().Name, ((ProductEntity)result.Items.First()).Name);
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
            var projection = repository.CreateProjection(_propertyNames);
            PaginatedList result = await repository.GetAllAsync(queryParameters, projection, _cacheService);

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
            var projection = repository.CreateProjection(_propertyNames);
            PaginatedList result = await repository.GetAllAsync(queryParameters, projection, _cacheService);

            List<ProductEntity> expectedProducts = products.Skip(20).Take(5).ToList();
            List<object> resultList = result.Items.ToList();

            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.ItemsCount);

            for (int i = 0; i < expectedProducts.Count; i++)
            {
                Assert.Equal(expectedProducts[i].Id, ((ProductEntity)resultList[i]).Id);
                Assert.Equal(expectedProducts[i].Name, ((ProductEntity)resultList[i]).Name);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            ProductEntity existingProducts = context.SeedOneProduct();
            Assert.NotNull(existingProducts);

            ProductRepository repository = new(context, _noxLoggerStub);
            var projection = repository.CreateProjection(_propertyNames);
            ProductEntity result = await repository.GetByIdAsync(existingProducts.Id, projection);

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
            var projection = repository.CreateProjection(_propertyNames);
            ProductEntity result = await repository.GetByIdAsync(existingProducts.Id, projection);

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

            ProductEntity product = new()
            {
                Name = "NewProduct"
            };

            await repository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            var projection = repository.CreateProjection(_propertyNames);
            var asd = context.Licenses.ToList();
            var result = await repository.GetByIdAsync(product.Id, projection);

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

            List<string> propertyList = ["Name"];
            repository.Update(existingProduct, propertyList);
            await unitOfWork.SaveChangesAsync();

            var projection = repository.CreateProjection(_propertyNames);
            var result = await repository.GetByIdAsync(existingProduct.Id, projection);

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

            repository.Remove(existingProduct);
            await unitOfWork.SaveChangesAsync();

            var projection = repository.CreateProjection(_propertyNames);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException<ProductEntity>>(async () =>
            {
                var result = await repository.GetByIdAsync(existingProduct.Id, projection);
            });

            Assert.NotNull(exception);
        }

        #endregion

        #endregion
    }
}