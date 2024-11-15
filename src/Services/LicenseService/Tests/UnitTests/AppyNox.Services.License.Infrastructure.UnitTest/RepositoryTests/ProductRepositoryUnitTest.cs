using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.Exceptions;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.UnitTest.Seeds.ProductSeeds;

namespace AppyNox.Services.License.Infrastructure.UnitTest.RepositoryTests;

public class ProductRepositoryUnitTest(RepositoryFixture fixture) : IClassFixture<RepositoryFixture>
{
    #region [ Fields ]

    private readonly NoxInfrastructureLoggerStub<UnitOfWorkBase> _noxLoggerStub = new();
    private readonly NoxInfrastructureLoggerStub<NoxRepositoryBase<ProductEntity>> _noxRepositoryLoggerStub = new();

    private readonly ICacheService _cacheService = fixture.RedisCacheService.Object;

    #endregion

    #region [ CRUD Methods ]

    #region [ Read ]

    [Fact]
    public async Task GetAllAsync_ShouldReturnEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedOneProduct(unitOfWork);
        ProductRepository repository = new(context, _noxRepositoryLoggerStub);
        QueryParameters queryParameters = new()
        {
            PageNumber = 1,
            PageSize = 1,
        };
        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnTwo()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleProducts(unitOfWork, 2);
        ProductRepository repository = new(context, _noxRepositoryLoggerStub);
        QueryParameters queryParameters = new()
        {
            PageNumber = 1,
            PageSize = 2,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Equal(2, result.ItemsCount);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnCorrectEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var products = await context.SeedMultipleProducts(unitOfWork, 2);
        Assert.NotNull(products);

        ProductRepository repository = new(context, _noxRepositoryLoggerStub);
        QueryParameters queryParameters = new()
        {
            PageNumber = 2,
            PageSize = 1,
        };
        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(products.Last().Id.Value, result.Items.First().Id.Value);
        Assert.Equal(products.Last().Name, result.Items.First().Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFifty()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleProducts(unitOfWork, 50);
        ProductRepository repository = new(context, _noxRepositoryLoggerStub);
        QueryParameters queryParameters = new()
        {
            PageNumber = 1,
            PageSize = 50,
        };
        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Equal(50, result.ItemsCount);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        IEnumerable<ProductEntity> products = await context.SeedMultipleProducts(unitOfWork, 50);
        Assert.NotNull(products);
        ProductRepository repository = new(context, _noxRepositoryLoggerStub);
        QueryParameters queryParameters = new()
        {
            PageNumber = 5,
            PageSize = 5,
        };
        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        List<ProductEntity> expectedProducts = products.Skip(20).Take(5).ToList();

        Assert.NotNull(result);
        Assert.Equal(expectedProducts.Count, result.ItemsCount);

        for (int i = 0; i < expectedProducts.Count; i++)
        {
            Assert.Equal(expectedProducts[i].Id.Value, result.Items.ElementAt(i).Id.Value);
            Assert.Equal(expectedProducts[i].Name, result.Items.ElementAt(i).Name);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        ProductEntity existingProducts = await context.SeedOneProduct(unitOfWork);
        Assert.NotNull(existingProducts);

        ProductRepository repository = new(context, _noxRepositoryLoggerStub);
        var result = await repository.GetByIdAsync(existingProducts.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldValuesBeCorrect()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        ProductEntity existingProducts = await context.SeedOneProduct(unitOfWork);
        Assert.NotNull(existingProducts);

        ProductRepository repository = new(context, _noxRepositoryLoggerStub);
        var result = await repository.GetByIdAsync(existingProducts.Id);

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
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        ProductRepository repository = new(context, _noxRepositoryLoggerStub);

        ProductEntity product = ProductEntity.Create("NewProduct");

        await repository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id.Value, result.Id.Value);
        Assert.Equal(product.Code, result.Code);
        Assert.Equal(product.Name, result.Name);
    }

    #endregion

    #region [ Update ]

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingProduct = await context.SeedOneProduct(unitOfWork);
        Assert.NotNull(existingProduct);

        ProductRepository repository = new(context, _noxRepositoryLoggerStub);

        existingProduct.Name = "NewProductUp";
        existingProduct.Code = "ORP01";

        repository.Update(existingProduct);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(existingProduct.Id);

        Assert.NotNull(result);
        Assert.Equal(existingProduct.Id.Value, result.Id.Value);
        Assert.Equal(existingProduct.Code, result.Code);
        Assert.Equal(existingProduct.Name, result.Name);
    }

    #endregion

    #region [ Delete ]

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingProduct = await context.SeedOneProduct(unitOfWork);
        Assert.NotNull(existingProduct);

        ProductRepository repository = new(context, _noxRepositoryLoggerStub);

        await repository.RemoveByIdAsync(existingProduct.Id);
        await unitOfWork.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<NoxEntityNotFoundException<ProductEntity>>(async () =>
        {
            var result = await repository.GetByIdAsync(existingProduct.Id);
        });

        Assert.NotNull(exception);
    }

    #endregion

    #endregion
}