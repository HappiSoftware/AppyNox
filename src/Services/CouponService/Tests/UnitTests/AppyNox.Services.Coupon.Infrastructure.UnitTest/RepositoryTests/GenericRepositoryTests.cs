using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds.CouponSeeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var repository = new GenericRepository<CouponEntity>(_context);
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
            var repository = new GenericRepository<CouponEntity>(_context);
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
            var context2 = new CouponDbContext(new DbContextOptionsBuilder<CouponDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);
            var coupons = RaiseSeedMultipleCoupons(context2, 2, 1);
            Assert.NotNull(coupons);

            var repository = new GenericRepository<CouponEntity>(context2);
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
            var test = _context.Coupons.ToList();
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(coupons.Last(), result.First());
        }

        [Fact, Order(4)]
        public async Task GetAllAsync_ShouldPaginationReturnFifty()
        {
            RaiseSeedMultipleCoupons(_context, 50, 5);
            var repository = new GenericRepository<CouponEntity>(_context);
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
            var context2 = new CouponDbContext(new DbContextOptionsBuilder<CouponDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);
            var coupons = RaiseSeedMultipleCoupons(context2, 50, 5);
            Assert.NotNull(coupons);

            var repository = new GenericRepository<CouponEntity>(context2);
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
            var test = context2.Coupons.ToList();
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
            Assert.Equal(coupons.Skip(20).Take(5), result);
        }

        [Fact, Order(6)]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var existingCoupon = RaiseSeedOneCoupon(_context);
            Assert.NotNull(existingCoupon);

            var repository = new GenericRepository<CouponEntity>(_context);
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

            var repository = new GenericRepository<CouponEntity>(_context);
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
            var unitOfWork = new UnitOfWorkBase(_context);
            var repository = new GenericRepository<CouponEntity>(_context);

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

            var unitOfWork = new UnitOfWorkBase(_context);
            var repository = new GenericRepository<CouponEntity>(_context);

            existingCoupon.Code = "TEE20";
            existingCoupon.Description = "DescriptionCouponUpdated";
            existingCoupon.DiscountAmount = 0.1;
            existingCoupon.MinAmount = 1;
            existingCoupon.CouponDetailEntityId = existingCoupon2.CouponDetailEntityId;

            repository.UpdateAsync(existingCoupon);
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

            var unitOfWork = new UnitOfWorkBase(_context);
            var repository = new GenericRepository<CouponEntity>(_context);

            repository.DeleteAsync(existingCoupon);
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