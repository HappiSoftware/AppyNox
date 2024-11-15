using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.Exceptions;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.UnitTest.Seeds.LicenseSeeds;

namespace AppyNox.Services.License.Infrastructure.UnitTest.RepositoryTests;

public class LicenseRepositoryUnitTest(RepositoryFixture fixture) : IClassFixture<RepositoryFixture>
{
    #region [ Fields ]

    private readonly RepositoryFixture _fixture = fixture;

    private readonly NoxInfrastructureLoggerStub<UnitOfWorkBase> _noxLoggerStub = new();
    private readonly NoxInfrastructureLoggerStub<NoxRepositoryBase<LicenseEntity>> _noxRepositoryLogger = new();

    private readonly ICacheService _cacheService = fixture.RedisCacheService.Object;

    #endregion

    #region [ CRUD Methods ]

    #region [ Read ]

    [Fact]
    public async Task GetAllAsync_ShouldReturnEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedOneLicense(unitOfWork);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);
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
        await context.SeedMultipleLicenses(unitOfWork, 2);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);
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
        var licenses = await context.SeedMultipleLicenses(unitOfWork, 2);
        Assert.NotNull(licenses);

        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);
        QueryParameters queryParameters = new()
        {
            PageNumber = 2,
            PageSize = 1,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Single(result.Items);

        Assert.Equal(licenses.Last().Id.Value, result.Items.First().Id.Value);
        Assert.Equal(licenses.Last().Code, result.Items.First().Code);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFifty()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleLicenses(unitOfWork, 50);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);
        QueryParameters queryParameters = new()
        {
            PageNumber = 1,
            PageSize = 50,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        Assert.NotNull(result);
        Assert.Equal(50, result.Items.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        IEnumerable<LicenseEntity> licenses = await context.SeedMultipleLicenses(unitOfWork, 50);
        Assert.NotNull(licenses);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);
        QueryParameters queryParameters = new()
        {
            PageNumber = 5,
            PageSize = 5,
        };

        var result = await repository.GetAllAsync(queryParameters, _cacheService);

        List<LicenseEntity> expectedLicenses = licenses.Skip(20).Take(5).ToList();

        Assert.NotNull(result);
        Assert.Equal(expectedLicenses.Count, result.ItemsCount);

        for (int i = 0; i < expectedLicenses.Count; i++)
        {
            Assert.Equal(expectedLicenses[i].Id.Value, result.Items.ElementAt(i).Id.Value);
            Assert.Equal(expectedLicenses[i].Code, result.Items.ElementAt(i).Code);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        LicenseEntity existingLicense = await context.SeedOneLicense(unitOfWork);
        Assert.NotNull(existingLicense);

        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);

        var result = await repository.GetByIdAsync(existingLicense.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldValuesBeCorrect()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        LicenseEntity existingLicense = await context.SeedOneLicense(unitOfWork);
        Assert.NotNull(existingLicense);

        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);
        LicenseEntity result = await repository.GetByIdAsync(existingLicense.Id);

        Assert.NotNull(result);
        Assert.Equal(existingLicense.Code, result.Code);
        Assert.Equal(existingLicense.Description, result.Description);
        Assert.Equal(existingLicense.LicenseKey, result.LicenseKey);
        Assert.Equal(existingLicense.ExpirationDate, result.ExpirationDate);
        Assert.Equal(existingLicense.MaxUsers, result.MaxUsers);
    }

    #endregion

    #region [ Create ]

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);

        LicenseEntity license = LicenseEntity.Create
        (
            "LK000",
            "DescriptionLicense",
            Guid.NewGuid().ToString(),
            DateTime.UtcNow.AddDays(2),
            1,
            2,
            Guid.NewGuid(),
            new ProductId(Guid.NewGuid())
        );

        await repository.AddAsync(license);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(license.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(license.Code, result.Code);
        Assert.Equal(license.Description, result.Description);
        Assert.Equal(license.LicenseKey, result.LicenseKey);
        Assert.Equal(license.ExpirationDate, result.ExpirationDate);
        Assert.Equal(license.MaxUsers, result.MaxUsers);
    }

    #endregion

    #region [ Update ]

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingLicense = await context.SeedOneLicense(unitOfWork);
        Assert.NotNull(existingLicense);

        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);

        existingLicense.Code = "LK000";
        existingLicense.Description = "DescriptionLicenseUpdated";
        existingLicense.LicenseKey = Guid.NewGuid().ToString();
        existingLicense.ExpirationDate = DateTime.UtcNow.AddDays(20);
        existingLicense.MaxUsers = 12;

        repository.Update(existingLicense);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(existingLicense.Id);

        Assert.NotNull(result);
        Assert.Equal(existingLicense.Code, result.Code);
        Assert.Equal(existingLicense.Description, result.Description);
        Assert.Equal(existingLicense.LicenseKey, result.LicenseKey);
        Assert.Equal(existingLicense.ExpirationDate, result.ExpirationDate);
        Assert.Equal(existingLicense.MaxUsers, result.MaxUsers);
    }

    #endregion

    #region [ Delete ]

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingLicense = await context.SeedOneLicense(unitOfWork);
        Assert.NotNull(existingLicense);

        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);

        await repository.RemoveByIdAsync(existingLicense.Id);
        await unitOfWork.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<NoxEntityNotFoundException<LicenseEntity>>(async () =>
        {
            var result = await repository.GetByIdAsync(existingLicense.Id);
        });

        Assert.NotNull(exception);
    }

    #endregion

    #endregion

    #region [ Custom Methods ]

    [Fact]
    public async Task FindLicenseByKeyAsync_ShouldReturnEntity()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedOneLicense(unitOfWork);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);

        LicenseEntity? entity = context.Licenses.FirstOrDefault();
        Assert.NotNull(entity);

        LicenseEntity? result = await repository.FindLicenseByKeyAsync(entity.LicenseKey, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetUserCountForLicenseKeyAsync_ShouldReturnCorrect(int userCount)
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedOneLicense(unitOfWork);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);

        LicenseEntity? entity = context.Licenses.FirstOrDefault();
        Assert.NotNull(entity);

        for (int index = 0; index < userCount; index++)
        {
            Guid mockUserId = Guid.NewGuid();
            await repository.AssignLicenseToApplicationUserAsync(entity.Id.Value, mockUserId);
        }

        int result = await repository.GetUserCountForLicenseKeyAsync(entity.Id.Value, CancellationToken.None);
        Assert.Equal(result, userCount);
    }

    [Fact]
    public async Task AssignLicenseToApplicationUserAsync_ShouldSuccess()
    {
        LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedOneLicense(unitOfWork);
        LicenseRepository repository = new(context, _noxRepositoryLogger, unitOfWork);

        LicenseEntity? entity = context.Licenses.FirstOrDefault();
        Assert.NotNull(entity);
        Guid mockUserId = Guid.NewGuid();

        await repository.AssignLicenseToApplicationUserAsync(entity.Id.Value, mockUserId);
        ApplicationUserLicenses? applicationUserLicense = context.ApplicationUserLicenses.Where(aul => aul.LicenseId == entity.Id && aul.UserId == mockUserId).FirstOrDefault();
        Assert.NotNull(applicationUserLicense);
    }

    #endregion
}