using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AppyNox.Services.Coupon.Domain.Exceptions.Base;
using AppyNox.Services.Coupon.Domain.Localization;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Infrastructure.UnitTest.Seeds;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;
using CouponAggregate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Infrastructure.UnitTest.RepositoryTests;

public class CouponRepositoryUnitTest : IClassFixture<RepositoryFixture>
{
    #region [ Fields ]

    private readonly RepositoryFixture _fixture;

    private readonly NoxInfrastructureLoggerStub _noxLoggerStub;

    private readonly ICacheService _cacheService;

    #endregion

    #region [ Public Constructors ]

    public CouponRepositoryUnitTest(RepositoryFixture fixture)
    {
        _fixture = fixture;
        _noxLoggerStub = fixture.NoxLoggerStub;
        _cacheService = fixture.RedisCacheService.Object;
        var localizer = new Mock<IStringLocalizer>();
        localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

        var localizerFactory = new Mock<IStringLocalizerFactory>();
        localizerFactory.Setup(lf => lf.Create(typeof(CouponDomainResourceService))).Returns(localizer.Object);

        CouponDomainResourceService.Initialize(localizerFactory.Object);
    }

    #endregion

    #region [ CRUD Methods ]

    #region [ Read ]

    [Theory]
    [InlineData(typeof(CouponAggregate))]
    [InlineData(typeof(CouponSimpleDto))]
    public async Task GetAllAsync_ShouldReturnEntity(Type fetchType)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedOneCoupon(unitOfWork);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 1,
            PageSize = 1,
        };

        var result = await repository.GetAllAsync(queryParameters, fetchType, _cacheService);

        Assert.NotNull(result);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnTwo()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 1,
            PageSize = 2,
        };

        var result = await repository.GetAllAsync(queryParameters, typeof(CouponSimpleDto), _cacheService);

        Assert.NotNull(result);
        Assert.Equal(2, result.ItemsCount);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnCorrectEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var coupons = await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        Assert.NotNull(coupons);

        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 2,
            PageSize = 1,
        };

        var result = await repository.GetAllAsync(queryParameters, typeof(CouponWithAllRelationsDto), _cacheService);

        Assert.NotNull(result);
        Assert.Single(result.Items);

        Assert.Equal(coupons.Last().Id.Value, ((CouponWithAllRelationsDto)result.Items.First()).Id.Value);
        Assert.Equal(coupons.Last().Code, ((CouponWithAllRelationsDto)result.Items.First()).Code);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFifty()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 50, 5);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 1,
            PageSize = 50,
        };

        var result = await repository.GetAllAsync(queryParameters, typeof(CouponSimpleDto), _cacheService);

        Assert.NotNull(result);
        Assert.Equal(50, result.ItemsCount);
    }

    [Fact]
    public async Task GetAllAsync_ShouldPaginationReturnFiftyAndCorrectEntities()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var coupons = await context.SeedMultipleCoupons(unitOfWork, 50, 5);
        Assert.NotNull(coupons);

        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "Simple",
            PageNumber = 5,
            PageSize = 5,
        };

        var result = await repository.GetAllAsync(queryParameters, typeof(CouponSimpleUpdateDto), _cacheService);

        var expectedCoupons = coupons.Skip(20).Take(5).ToList();
        var resultList = result.Items.ToList();

        Assert.NotNull(result);
        Assert.Equal(expectedCoupons.Count, result.ItemsCount);

        for (int i = 0; i < expectedCoupons.Count; i++)
        {
            Assert.Equal(expectedCoupons[i].Id.Value, ((CouponSimpleUpdateDto)resultList[i]).Id.Value);
            Assert.Equal(expectedCoupons[i].Code, ((CouponSimpleUpdateDto)resultList[i]).Code);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingCoupon = await context.SeedOneCoupon(unitOfWork);
        Assert.NotNull(existingCoupon);

        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        var result = await repository.GetByIdAsync(existingCoupon.Id, typeof(CouponSimpleDto));

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldValuesBeCorrect()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingCoupon = await context.SeedOneCoupon(unitOfWork);
        Assert.NotNull(existingCoupon);

        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);

        var result = await repository.GetByIdAsync(existingCoupon.Id, typeof(CouponWithAllRelationsDto)) as CouponWithAllRelationsDto;

        Assert.NotNull(result);

        Assert.Equal(existingCoupon.Code, result.Code);
        Assert.Equal(existingCoupon.Description, result.Description);
        Assert.Equal(existingCoupon.Amount.DiscountAmount, result.Amount.DiscountAmount);
        Assert.Equal(existingCoupon.Amount.MinAmount, result.Amount.MinAmount);
        Assert.Equal(existingCoupon.CouponDetailId.Value, result.CouponDetailId.Value);
    }

    #endregion

    #region [ Create ]

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);

        var existingCoupon = await context.SeedOneCoupon(unitOfWork); // To Seed CouponDetail
        Assert.NotNull(existingCoupon);

        Amount amount = new(0.1, 1);
        CouponAggregate coupon = new CouponBuilder().WithDetails("TEE10", "DescriptionCoupon", "")
                                                    .WithAmount(amount)
                                                    .WithCouponDetailId(existingCoupon.CouponDetailId)
                                                    .Build();

        await repository.AddAsync(coupon);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(coupon.Id, typeof(CouponWithAllRelationsDto)) as CouponWithAllRelationsDto;

        Assert.NotNull(result);

        Assert.Equal(coupon.Id.Value, result.Id.Value);
        Assert.Equal(coupon.Code, result.Code);
        Assert.Equal(coupon.Description, result.Description);
        Assert.Equal(coupon.Amount.DiscountAmount, result.Amount.DiscountAmount);
        Assert.Equal(coupon.Amount.MinAmount, result.Amount.MinAmount);
        Assert.Equal(coupon.CouponDetailId.Value, result.CouponDetailId.Value);
    }

    #endregion

    #region [ Update ]

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMinimumAmount()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingCoupon = await context.SeedOneCoupon(unitOfWork);
        Assert.NotNull(existingCoupon);

        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);

        existingCoupon.UpdateMinimumAmount(10);

        repository.Update(existingCoupon);
        await unitOfWork.SaveChangesAsync();

        var result = await repository.GetByIdAsync(existingCoupon.Id, typeof(CouponWithAllRelationsDto)) as CouponWithAllRelationsDto;

        Assert.NotNull(result);

        Assert.Equal(existingCoupon.Id.Value, result.Id.Value);
        Assert.Equal(existingCoupon.Code, result.Code);
        Assert.Equal(existingCoupon.Description, result.Description);
        Assert.Equal(existingCoupon.Amount.DiscountAmount, result.Amount.DiscountAmount);
        Assert.Equal(existingCoupon.Amount.MinAmount, result.Amount.MinAmount);
        Assert.Equal(existingCoupon.CouponDetailId.Value, result.CouponDetailId.Value);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnExceptionUpdatingMinimumAmount()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingCoupon = await context.SeedOneCoupon(unitOfWork);
        Assert.NotNull(existingCoupon);

        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);

        var exception = Assert.Throws<NoxCouponDomainException>(() => existingCoupon.UpdateMinimumAmount(0));
        Assert.Equal((int)HttpStatusCode.UnprocessableContent, exception.StatusCode);
        Assert.Equal((int)NoxCouponDomainExceptionCode.AmountValidation, exception.ExceptionCode);
    }

    #endregion

    #region [ Delete ]

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        var existingCoupon = await context.SeedOneCoupon(unitOfWork);
        Assert.NotNull(existingCoupon);

        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);

        await repository.RemoveByIdAsync(existingCoupon.Id);
        await unitOfWork.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<EntityNotFoundException<CouponAggregate>>(async () =>
        {
            var result = await repository.GetByIdAsync(existingCoupon.Id, typeof(CouponSimpleDto));
        });

        Assert.NotNull(exception);
    }

    #endregion

    #endregion
}