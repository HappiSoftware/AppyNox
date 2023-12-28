using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using Xunit.Extensions.Ordering;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.RepositoryTests
{
    public class GenericRepositoryTests : InfrastructureTestBase, IDisposable
    {
        #region [ CRUD Methods ]

        #region [ Read ]

        [Fact, Order(1)]
        public async Task GetAllAsync_ShouldReturnEntity()
        {
            RaiseSeedOneCoupon(_context);
            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);
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

        [Fact, Order(2)]
        public async Task GetAllAsync_ShouldPaginationReturnTwo()
        {
            RaiseSeedMultipleCoupons(_context, 2, 1);
            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);
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

        [Fact, Order(3)]
        public async Task GetAllAsync_ShouldPaginationReturnCorrectEntity()
        {
            var coupons = RaiseSeedMultipleCoupons(_context, 2, 1);
            Assert.NotNull(coupons);

            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);
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

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(coupons.Last().Id, ((CouponEntity)result.First()).Id);
            Assert.Equal(coupons.Last().Code, ((CouponEntity)result.First()).Code);
        }

        [Fact, Order(4)]
        public async Task GetAllAsync_ShouldPaginationReturnFifty()
        {
            RaiseSeedMultipleCoupons(_context, 50, 5);
            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);
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

        [Fact, Order(5)]
        public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
        {
            var coupons = RaiseSeedMultipleCoupons(_context, 50, 5);
            Assert.NotNull(coupons);

            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);
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

        [Fact, Order(6)]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var existingCoupon = RaiseSeedOneCoupon(_context);
            Assert.NotNull(existingCoupon);

            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);
            var propertyNames = typeof(CouponEntity).GetProperties().Select(p => p.Name).ToList();
            var projection = repository.CreateProjection(propertyNames);
            var result = await repository.GetByIdAsync(existingCoupon.Id, projection);

            Assert.NotNull(result);
        }

        [Fact, Order(7)]
        public async Task GetByIdAsync_ShouldValuesBeCorrect()
        {
            var existingCoupon = RaiseSeedOneCoupon(_context);
            Assert.NotNull(existingCoupon);

            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);
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

        [Fact, Order(8)]
        public async Task AddAsync_ShouldAddEntity()
        {
            var unitOfWork = new UnitOfWorkBase(_context, _noxLoggerStub);
            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);

            var existingCoupon = RaiseSeedOneCoupon(_context); // To Seed CouponDetail
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

        [Fact, Order(9)]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var existingCoupon = RaiseSeedOneCoupon(_context);
            var existingCoupon2 = RaiseSeedOneCoupon(_context); // For getting a second CouponDetailEntityId
            Assert.NotNull(existingCoupon);
            Assert.NotNull(existingCoupon2);

            var unitOfWork = new UnitOfWorkBase(_context, _noxLoggerStub);
            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);

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

        [Fact, Order(10)]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            var existingCoupon = RaiseSeedOneCoupon(_context);
            Assert.NotNull(existingCoupon);

            var unitOfWork = new UnitOfWorkBase(_context, _noxLoggerStub);
            var repository = new GenericRepository<CouponEntity>(_context, _noxLoggerStub);

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