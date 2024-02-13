using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.UnitTest.Seeds.LicenseSeeds;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AppyNox.Services.License.Infrastructure.UnitTest.RepositoryTests
{
    public class LicenseRepositoryUnitTest(RepositoryFixture fixture) : IClassFixture<RepositoryFixture>
    {
        #region [ Fields ]

        private readonly RepositoryFixture _fixture = fixture;

        private readonly NoxInfrastructureLoggerStub _noxLoggerStub = fixture.NoxLoggerStub;

        private readonly List<string> _propertyNames = typeof(LicenseEntity)
            .GetProperties()
            .Select(p => p.Name)
            .Where(name => name != "Product")
            .ToList();

        private readonly ICacheService _cacheService = fixture.RedisCacheService.Object;

        #endregion

        #region [ CRUD Methods ]

        #region [ Read ]

        [Fact]
        public async Task GetAllAsync_ShouldReturnEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);
            context.SeedOneLicense();
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);
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
            context.SeedMultipleLicenses(2);
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);
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
            var licenses = context.SeedMultipleLicenses(2);
            Assert.NotNull(licenses);

            UnitOfWork unitOfWork = new(context, _noxLoggerStub);
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);
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
            Assert.Equal(licenses.Last().Id, ((LicenseEntity)result.Items.First()).Id);
            Assert.Equal(licenses.Last().Code, ((LicenseEntity)result.Items.First()).Code);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFifty()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            context.SeedMultipleLicenses(50);
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);
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
            Assert.Equal(50, result.Items.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            IEnumerable<LicenseEntity> licenses = context.SeedMultipleLicenses(50);
            Assert.NotNull(licenses);
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 5,
                PageSize = 5,
            };
            var projection = repository.CreateProjection(_propertyNames);
            PaginatedList result = await repository.GetAllAsync(queryParameters, projection, _cacheService);

            List<LicenseEntity> expectedLicenses = licenses.Skip(20).Take(5).ToList();
            List<object> resultList = result.Items.ToList();

            Assert.NotNull(result);
            Assert.Equal(expectedLicenses.Count, result.ItemsCount);

            for (int i = 0; i < expectedLicenses.Count; i++)
            {
                Assert.Equal(expectedLicenses[i].Id, ((LicenseEntity)resultList[i]).Id);
                Assert.Equal(expectedLicenses[i].Code, ((LicenseEntity)resultList[i]).Code);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            LicenseEntity existingLicense = context.SeedOneLicense();
            Assert.NotNull(existingLicense);
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);

            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);
            var projection = repository.CreateProjection(_propertyNames);
            LicenseEntity result = await repository.GetByIdAsync(existingLicense.Id, projection);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldValuesBeCorrect()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            LicenseEntity existingLicense = context.SeedOneLicense();
            Assert.NotNull(existingLicense);
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);

            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);
            var projection = repository.CreateProjection(_propertyNames);
            LicenseEntity result = await repository.GetByIdAsync(existingLicense.Id, projection);

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
            var unitOfWork = new UnitOfWork(context, _noxLoggerStub);
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);

            LicenseEntity license = new()
            {
                Code = "LK000",
                Description = "DescriptionLicense",
                LicenseKey = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow.AddDays(2),
                MaxUsers = 1,
                ProductId = Guid.NewGuid()
            };

            await repository.AddAsync(license);
            await unitOfWork.SaveChangesAsync();

            var projection = repository.CreateProjection(_propertyNames);
            var asd = context.Licenses.ToList();
            var result = await repository.GetByIdAsync(license.Id, projection);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(license.Id, result.Id);
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
            var existingLicense = context.SeedOneLicense();
            Assert.NotNull(existingLicense);

            var unitOfWork = new UnitOfWork(context, _noxLoggerStub);
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);

            existingLicense.Code = "LK000";
            existingLicense.Description = "DescriptionLicenseUpdated";
            existingLicense.LicenseKey = Guid.NewGuid().ToString();
            existingLicense.ExpirationDate = DateTime.UtcNow.AddDays(20);
            existingLicense.MaxUsers = 12;

            List<string> propertyList = ["Code", "Description", "LicenseKey", "ExpirationDate", "MaxUsers"];
            repository.Update(existingLicense, propertyList);
            await unitOfWork.SaveChangesAsync();

            var projection = repository.CreateProjection(_propertyNames);
            var result = await repository.GetByIdAsync(existingLicense.Id, projection);

            Assert.NotNull(result);
            Assert.Equal(existingLicense.Id, result.Id);
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
            var existingLicense = context.SeedOneLicense();
            Assert.NotNull(existingLicense);

            var unitOfWork = new UnitOfWork(context, _noxLoggerStub);
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);

            repository.Remove(existingLicense);
            await unitOfWork.SaveChangesAsync();

            var projection = repository.CreateProjection(_propertyNames);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException<LicenseEntity>>(async () =>
            {
                var result = await repository.GetByIdAsync(existingLicense.Id, projection);
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
            context.SeedOneLicense();
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);

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
            context.SeedOneLicense();
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);

            LicenseEntity? entity = context.Licenses.FirstOrDefault();
            Assert.NotNull(entity);

            for (int index = 0; index < userCount; index++)
            {
                Guid mockUserId = Guid.NewGuid();
                await repository.AssignLicenseToApplicationUserAsync(entity.Id, mockUserId);
            }

            int result = await repository.GetUserCountForLicenseKeyAsync(entity.Id, CancellationToken.None);
            Assert.Equal(result, userCount);
        }

        [Fact]
        public async Task AssignLicenseToApplicationUserAsync_ShouldSuccess()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            UnitOfWork unitOfWork = new(context, _noxLoggerStub);
            context.SeedOneLicense();
            LicenseRepository repository = new(context, _noxLoggerStub, unitOfWork);

            LicenseEntity? entity = context.Licenses.FirstOrDefault();
            Assert.NotNull(entity);
            Guid mockUserId = Guid.NewGuid();

            await repository.AssignLicenseToApplicationUserAsync(entity.Id, mockUserId);
            ApplicationUserLicenses? applicationUserLicense = context.ApplicationUserLicenses.Where(aul => aul.LicenseId == entity.Id && aul.UserId == mockUserId).FirstOrDefault();
            Assert.NotNull(applicationUserLicense);
        }

        #endregion
    }
}