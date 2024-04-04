using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
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
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

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

    [Theory]
    [InlineData(typeof(CouponAggregate))]
    [InlineData(typeof(CouponWithAllRelationsDto))]
    public async Task GetAllAsync_ShouldApplySorting(Type fetchType)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "WithAllRelations",
            PageNumber = 1,
            PageSize = 2,
            SortBy = "code asc, CouponDetail.Detail desc"
        };

        var result = await repository.GetAllAsync(queryParameters, fetchType, _cacheService);

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
    }

    #region [ Sorting ]

    [Theory]
    [InlineData(typeof(CouponAggregate), "code asc2")]
    [InlineData(typeof(CouponSimpleDto), "test desc")]
    public async Task GetAllAsync_ShouldRefuseWrongSortBy(Type fetchType, string sortBy)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "WithAllRelations",
            PageNumber = 1,
            PageSize = 2,
            SortBy = sortBy
        };

        var exception = await Assert.ThrowsAsync<NoxInfrastructureException>(() => repository.GetAllAsync(queryParameters, fetchType, _cacheService));
        Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal(1008, exception.ExceptionCode);
    }

    [Theory]
    [InlineData("Amount.DiscountAmount", true)]
    [InlineData("Amount.DiscountAmount desc", false)]
    public async Task GetAllAsync_ShouldSortCorrectly(string sortBy, bool ascending)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "WithAllRelations",
            PageNumber = 1,
            PageSize = 2,
            SortBy = sortBy
        };

        var result = await repository.GetAllAsync(queryParameters, typeof(CouponSimpleDto), _cacheService);
        TypedPaginatedList<CouponSimpleDto> typedList = result.ConvertToTypedPaginatedList<CouponSimpleDto>();
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        if (ascending)
        {
            Assert.True(typedList.Items.ElementAt(0).Amount.DiscountAmount < typedList.Items.ElementAt(1).Amount.DiscountAmount);
        }
        else
        {
            Assert.True(typedList.Items.ElementAt(0).Amount.DiscountAmount > typedList.Items.ElementAt(1).Amount.DiscountAmount);
        }
    }

    [Theory]
    [InlineData("drop insert delete update exec execute merge declare alter create xp_ sp_ -- " +
    "; /* */ cast convert table from select sysobjects syscolumns @@ db_name bulk admin grant" +
    " revoke deny link openquery opendatasource openrowset dump restore pg_ setval currval nextval" +
    " regclass :: plpythonu plperlu dblink pg_sleep lo_import lo_export pg_read_file pg_ls_dir")]
    public async Task GetAllAsync_ShouldRefuseMaliciousSort(string sort)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "WithAllRelations",
            PageNumber = 1,
            PageSize = 2,
            SortBy = sort
        };

        var exception = await Assert.ThrowsAsync<NoxInfrastructureException>(() => repository.GetAllAsync(queryParameters, typeof(CouponSimpleDto), _cacheService));
        Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal(1009, exception.ExceptionCode);
    }

    #endregion

    #region [ Filtering ]

    [Theory]
    [InlineData("Amount.DiscountAmount == 10", 1)]
    [InlineData("code == \"EXF10\"", 1)]
    [InlineData("code.Contains(\"EXF\")", 3)]
    [InlineData("code.Contains(\"EXF\") and Amount.DiscountAmount == 10", 1)]
    [InlineData("code.Contains(\"EXF\") and (Amount.DiscountAmount >= 10 and Amount.MinAmount == 102)", 1)]
    public async Task GetAllAsync_ShouldApplyFiltering(string filter, int size)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 3, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "WithAllRelations",
            PageNumber = 1,
            PageSize = 3,
            Filter = filter
        };

        var result = await repository.GetAllAsync(queryParameters, typeof(CouponAggregate), _cacheService);

        Assert.NotNull(result);
        Assert.Equal(size, result.Items.Count());
    }

    [Theory]
    [InlineData("code asc2")]
    [InlineData("test == 10")]
    public async Task GetAllAsync_ShouldRefuseWrongFilter(string filter)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "WithAllRelations",
            PageNumber = 1,
            PageSize = 2,
            Filter = filter
        };

        var exception = await Assert.ThrowsAsync<NoxInfrastructureException>(() => repository.GetAllAsync(queryParameters, typeof(CouponSimpleDto), _cacheService));
        Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal(1008, exception.ExceptionCode);
    }

    [Theory]
    [InlineData("drop insert delete update exec execute merge declare alter create xp_ sp_ -- " +
        "; /* */ cast convert table from select sysobjects syscolumns @@ db_name bulk admin grant" +
        " revoke deny link openquery opendatasource openrowset dump restore pg_ setval currval nextval" +
        " regclass :: plpythonu plperlu dblink pg_sleep lo_import lo_export pg_read_file pg_ls_dir")]
    public async Task GetAllAsync_ShouldRefuseMaliciousFilter(string filter)
    {
        CouponDbContext context = RepositoryFixture.CreateDatabaseContext<CouponDbContext>();
        UnitOfWork unitOfWork = new(context, _noxLoggerStub);
        await context.SeedMultipleCoupons(unitOfWork, 2, 1);
        var repository = new CouponRepository<CouponAggregate>(context, _noxLoggerStub);
        QueryParameters queryParameters = new()
        {
            Access = string.Empty,
            DetailLevel = "WithAllRelations",
            PageNumber = 1,
            PageSize = 2,
            Filter = filter
        };

        var exception = await Assert.ThrowsAsync<NoxInfrastructureException>(() => repository.GetAllAsync(queryParameters, typeof(CouponSimpleDto), _cacheService));
        Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal(1009, exception.ExceptionCode);
    }

    #endregion

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