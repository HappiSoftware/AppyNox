using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data;
using AppyNox.Services.License.Infrastructure.Repositories;
using AppyNox.Services.License.Infrastructure.UnitTest.Seeds.LicenseSeeds;

namespace AppyNox.Services.License.Infrastructure.UnitTest.RepositoryTests
{
    public class GenericRepositoryTests(RepositoryFixture fixture) : IClassFixture<RepositoryFixture>
    {
        #region [ Fields ]

        private readonly RepositoryFixture _fixture = fixture;

        private readonly NoxInfrastructureLoggerStub _noxLoggerStub = fixture.NoxLoggerStub;

        #endregion

        #region [ CRUD Methods ]

        #region [ Read ]

        [Fact]
        public async Task GetAllAsync_ShouldReturnEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            context.SeedOneLicense();
            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 1,
            };
            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnTwo()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            context.SeedMultipleLicenses(2);
            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 2,
            };
            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnCorrectEntity()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            var licenses = context.SeedMultipleLicenses(2);
            Assert.NotNull(licenses);

            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 2,
                PageSize = 1,
            };
            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(licenses.Last().Id, ((LicenseEntity)result.First()).Id);
            Assert.Equal(licenses.Last().Code, ((LicenseEntity)result.First()).Code);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFifty()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            context.SeedMultipleLicenses(50);
            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 50,
            };
            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            Assert.NotNull(result);
            Assert.Equal(50, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            var licenses = context.SeedMultipleLicenses(50);
            Assert.NotNull(licenses);

            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 5,
                PageSize = 5,
            };
            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            var expectedLicenses = licenses.Skip(20).Take(5).ToList();
            var resultList = result.ToList();

            Assert.NotNull(result);
            Assert.Equal(expectedLicenses.Count, result.Count());

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
            var existingLicense = context.SeedOneLicense();
            Assert.NotNull(existingLicense);

            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);
            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(existingLicense.Id, projection);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldValuesBeCorrect()
        {
            LicenseDatabaseContext context = RepositoryFixture.CreateDatabaseContext<LicenseDatabaseContext>();
            var existingLicense = context.SeedOneLicense();
            Assert.NotNull(existingLicense);

            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);
            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(existingLicense.Id, projection);

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
            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);

            LicenseEntity license = new()
            {
                Code = "LK000",
                Description = "DescriptionLicense",
                LicenseKey = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow.AddDays(2),
                MaxUsers = 1,
            };

            await repository.AddAsync(license);
            await unitOfWork.SaveChangesAsync();

            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(license.Id, projection);

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
            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);

            existingLicense.Code = "LK000";
            existingLicense.Description = "DescriptionLicenseUpdated";
            existingLicense.LicenseKey = Guid.NewGuid().ToString();
            existingLicense.ExpirationDate = DateTime.UtcNow.AddDays(20);
            existingLicense.MaxUsers = 12;

            List<string> propertyList = ["Code", "Description", "LicenseKey", "ExpirationDate", "MaxUsers"];
            repository.Update(existingLicense, propertyList);
            await unitOfWork.SaveChangesAsync();

            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
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
            var repository = new GenericRepository<LicenseEntity>(context, _noxLoggerStub);

            repository.Remove(existingLicense);
            await unitOfWork.SaveChangesAsync();

            var propertyNames = typeof(LicenseEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException<LicenseEntity>>(async () =>
            {
                var result = await repository.GetByIdAsync(existingLicense.Id, projection);
            });

            Assert.NotNull(exception);
        }

        #endregion

        #endregion
    }
}