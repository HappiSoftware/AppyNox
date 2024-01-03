using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds.CouponSeeds;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.RepositoryTests
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
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            context.SeedOneCoupon();
            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 1,
            };
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnTwo()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            context.SeedMultipleCoupons(2, 1);
            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 2,
            };
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnCorrectEntity()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            var coupons = context.SeedMultipleCoupons(2, 1);
            Assert.NotNull(coupons);

            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 2,
                PageSize = 1,
            };
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            var test = context.Coupons.ToList();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(coupons.Last().Id, ((CouponEntity)result.First()).Id);
            Assert.Equal(coupons.Last().Code, ((CouponEntity)result.First()).Code);
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFifty()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            context.SeedMultipleCoupons(50, 5);
            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 1,
                PageSize = 50,
            };
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            Assert.NotNull(result);
            Assert.Equal(50, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            var coupons = context.SeedMultipleCoupons(50, 5);
            Assert.NotNull(coupons);

            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "Simple",
                PageNumber = 5,
                PageSize = 5,
            };
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetAllAsync(queryParameters, projection);

            var expectedCoupons = coupons.Skip(20).Take(5).ToList();
            var resultList = result.ToList();

            Assert.NotNull(result);
            Assert.Equal(expectedCoupons.Count, result.Count());

            for (int i = 0; i < expectedCoupons.Count; i++)
            {
                Assert.Equal(expectedCoupons[i].Id, ((CouponEntity)resultList[i]).Id);
                Assert.Equal(expectedCoupons[i].Code, ((CouponEntity)resultList[i]).Code);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            var existingCoupon = context.SeedOneCoupon();
            Assert.NotNull(existingCoupon);

            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(existingCoupon.Id, projection);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldValuesBeCorrect()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            var existingCoupon = context.SeedOneCoupon();
            Assert.NotNull(existingCoupon);

            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(existingCoupon.Id, projection);

            Assert.NotNull(result);
            Assert.Equal(existingCoupon.Code, result.Code);
            Assert.Equal(existingCoupon.Description, result.Description);
            Assert.Equal(existingCoupon.DiscountAmount, result.DiscountAmount);
            Assert.Equal(existingCoupon.MinAmount, result.MinAmount);
            Assert.Equal(existingCoupon.CouponDetailEntityId, result.CouponDetailEntityId);
        }

        #endregion

        #region [ Create ]

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            var unitOfWork = new UnitOfWorkBase(context, _noxLoggerStub);
            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);

            var existingCoupon = context.SeedOneCoupon(); // To Seed CouponDetail
            Assert.NotNull(existingCoupon);

            CouponEntity coupon = new()
            {
                Code = "TEE10",
                Description = "DescriptionCoupon",
                DiscountAmount = 0.1,
                MinAmount = 1,
                CouponDetailEntityId = existingCoupon.CouponDetailEntityId
            };

            await repository.AddAsync(coupon);
            await unitOfWork.SaveChangesAsync();

            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(coupon.Id, projection);

            Assert.NotNull(result);
            Assert.Equal(coupon.Id, result.Id);
            Assert.Equal(coupon.Code, result.Code);
            Assert.Equal(coupon.Description, result.Description);
            Assert.Equal(coupon.DiscountAmount, result.DiscountAmount);
            Assert.Equal(coupon.MinAmount, result.MinAmount);
            Assert.Equal(coupon.CouponDetailEntityId, result.CouponDetailEntityId);
        }

        #endregion

        #region [ Update ]

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            var existingCoupon = context.SeedOneCoupon();
            var existingCoupon2 = context.SeedOneCoupon(); // For getting a second CouponDetailEntityId
            Assert.NotNull(existingCoupon);
            Assert.NotNull(existingCoupon2);

            var unitOfWork = new UnitOfWorkBase(context, _noxLoggerStub);
            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);

            existingCoupon.Code = "TEE20";
            existingCoupon.Description = "DescriptionCouponUpdated";
            existingCoupon.DiscountAmount = 0.1;
            existingCoupon.MinAmount = 1;
            existingCoupon.CouponDetailEntityId = existingCoupon2.CouponDetailEntityId;

            List<string> propertyList = ["Code", "Description", "DiscountAmount", "MinAmount", "CouponDetailEntityId"];
            repository.Update(existingCoupon, propertyList);
            await unitOfWork.SaveChangesAsync();

            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(existingCoupon.Id, projection);

            Assert.NotNull(result);
            Assert.Equal(existingCoupon.Id, result.Id);
            Assert.Equal(existingCoupon.Code, result.Code);
            Assert.Equal(existingCoupon.Description, result.Description);
            Assert.Equal(existingCoupon.DiscountAmount, result.DiscountAmount);
            Assert.Equal(existingCoupon.MinAmount, result.MinAmount);
            Assert.Equal(existingCoupon.CouponDetailEntityId, result.CouponDetailEntityId);
        }

        #endregion

        #region [ Delete ]

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            CouponDbContext context = _fixture.CreateDatabaseContext<CouponDbContext>();
            var existingCoupon = context.SeedOneCoupon();
            Assert.NotNull(existingCoupon);

            var unitOfWork = new UnitOfWorkBase(context, _noxLoggerStub);
            var repository = new GenericRepository<CouponEntity>(context, _noxLoggerStub);

            repository.Remove(existingCoupon);
            await unitOfWork.SaveChangesAsync();

            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException<CouponEntity>>(async () =>
            {
                var result = await repository.GetByIdAsync(existingCoupon.Id, projection);
            });

            Assert.NotNull(exception);
        }

        #endregion

        #endregion
    }
}