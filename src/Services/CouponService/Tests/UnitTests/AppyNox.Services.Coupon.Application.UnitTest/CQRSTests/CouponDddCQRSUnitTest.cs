using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Application.MediatR;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AppyNox.Services.Coupon.Domain.Localization;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using System.Text.Json;
using CouponAggregate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;
using NCEHelper = AppyNox.Services.Base.Application.UnitTests.Helpers.NoxCommandExtensionsHelper;

namespace AppyNox.Services.Coupon.Application.UnitTest.CQRSTests;

public class CouponDddCQRSUnitTest : IClassFixture<NoxApplicationTestFixture>
{
    #region [ Fields ]

    private readonly NoxApplicationTestFixture _fixture;

    private readonly IServiceProvider _serviceProvider;

    private readonly IMediator _mediator;

    #endregion

    #region [ Public Constructors ]

    public CouponDddCQRSUnitTest(NoxApplicationTestFixture fixture)
    {
        _fixture = fixture;

        NoxApplicationLoggerStub<CouponDddCQRSUnitTest> logger = new();
        Mock<INoxRepository<CouponAggregate>> mockRepository = new();
        if (!_fixture.DIInitialized)
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Consul:ServiceName", "TestService"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _fixture.ServiceCollection.AddCouponApplication(configuration, logger);
            _fixture.ServiceCollection.AddScoped(typeof(INoxRepository<CouponAggregate>), _ => mockRepository.Object);
            _fixture.DIInitialized = true;
        }

        _serviceProvider = _fixture.ServiceCollection.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();

        #region [ Repository Mocks ]

        CouponDetail newDetail = new CouponDetailBuilder().WithDetails("test01", "testdetail").Build();
        CouponAggregate couponEntity = new CouponBuilder().WithDetails("code", "description", "detail").WithAmount(10, 15).WithCouponDetail(newDetail).MarkAsBulkCreate().Build();
        CouponDto couponSimpleDto = new()
        {
            Code = "code",
            Description = "description",
            Amount = new AmountDto() { DiscountAmount = 10, MinAmount = 20 },
            CouponDetailId = new CouponDetailIdDto() { Value = Guid.NewGuid() }
        };
        mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<ICacheService>()))
            .ReturnsAsync(new Mock<PaginatedList<CouponAggregate>>().Object);

        mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<CouponId>(), false, false))
            .ReturnsAsync(couponEntity);

        mockRepository.Setup(repo => repo.AddAsync(It.IsAny<CouponAggregate>()))
            .ReturnsAsync(couponEntity);

        #endregion

        #region [ Localizer ]

        InitializeRealLocalizer();

        #endregion
    }

    #endregion

    #region [ Public Methods ]

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldSuccess()
    {
        // Act
        var result = await _mediator.Send(new GetAllNoxEntitiesQuery<CouponAggregate, CouponDto>(_fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetAllNoxEntitiesQuery<CouponAggregate, CouponDto>(_fixture.MockQueryParameters.Object, extensions));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldActionFail()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1), new(NCEHelper.DummyMethod2, RunType.After)]);

        // Act
        var exception = await Assert.ThrowsAsync<NoxApplicationException>(() => _mediator.Send(new GetAllNoxEntitiesQuery<CouponAggregate, CouponDto>(_fixture.MockQueryParameters.Object, extensions)));

        // Assert
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal(998, exception.ExceptionCode);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task GetEntityByIdQuery_ShouldSuccess()
    {
        // Act
        var result = await _mediator.Send(new GetNoxEntityByIdQuery<CouponAggregate, CouponId, CouponDto>(It.IsAny<CouponId>(), _fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is CouponDto);
    }

    [Fact]
    public async Task GetEntityByIdQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetNoxEntityByIdQuery<CouponAggregate, CouponId, CouponDto>(It.IsAny<CouponId>(), _fixture.MockQueryParameters.Object, Extensions: extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is CouponDto);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldSuccess()
    {
        // Prepare
        CouponCompositeCreateDto dto = new()
        {
            Code = "fffj9",
            Amount = new()
            {
                DiscountAmount = 2,
                MinAmount = 13
            },
            Description = "Description",
            CouponDetail = new()
            {
                Code = "Deta1",
                CouponDetailTags = [
                    new()
                    {
                        Tag = "Tag1"
                    },
                    new()
                    {
                        Tag = "Tag2"
                    }
                ]
            }
        };
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator.Send(new CreateNoxEntityCommand<CouponAggregate, CouponCompositeCreateDto>(dto));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Prepare
        CouponCompositeCreateDto dto = new()
        {
            Code = "fffj9",
            Amount = new()
            {
                DiscountAmount = 2,
                MinAmount = 13
            },
            Description = "Description",
            CouponDetail = new()
            {
                Code = "Deta1",
                CouponDetailTags = [
                    new()
                    {
                        Tag = "Tag1"
                    },
                    new()
                    {
                        Tag = "Tag2"
                    }
                ]
            }
        };
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator.Send(new CreateNoxEntityCommand<CouponAggregate, CouponCompositeCreateDto>(dto, extensions));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public void DeleteEntityCommand_ShouldSuccess()
    {
        // Act
        var result = _mediator
            .Send(new DeleteNoxEntityCommand<CouponAggregate, CouponId>(new CouponId(Guid.NewGuid()), false));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
    }


    // Detailed NoxCommandExtensions tests below

    [Fact]
    public void DeleteEntityCommand_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1), new(NCEHelper.DummyMethod2, suspendOnFailure: false)]);

        // Act
        var result = _mediator
            .Send(new DeleteNoxEntityCommand<CouponAggregate, CouponId>(new CouponId(Guid.NewGuid()), false, extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0B1M", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task DeleteEntityCommand_ShouldActionFail()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1), new(NCEHelper.DummyMethod2, RunType.After)]);

        // Act
        var exception = await Assert.ThrowsAsync<NoxApplicationException>(() => _mediator
            .Send(new DeleteNoxEntityCommand<CouponAggregate, CouponId>(new CouponId(Guid.NewGuid()), false, extensions)));
        
        // Assert
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal(998, exception.ExceptionCode);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public void DeleteEntityCommand_ShouldCallActionsAfter()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.After), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = _mediator
            .Send(new DeleteNoxEntityCommand<CouponAggregate, CouponId>(new CouponId(Guid.NewGuid()), false, extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("MA0A1", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public void DeleteEntityCommand_ShouldCallActionsMixed()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = _mediator
            .Send(new DeleteNoxEntityCommand<CouponAggregate, CouponId>(new CouponId(Guid.NewGuid()), false, extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    /// <summary>
    /// Same as the test above however we register the After action first.
    /// </summary>
    [Fact]
    public void DeleteEntityCommand_ShouldCallActionsMixed2()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.After), new(NCEHelper.DummyMethod2, RunType.Before, suspendOnFailure: false)]);

        // Act
        var result = _mediator
            .Send(new DeleteNoxEntityCommand<CouponAggregate, CouponId>(new CouponId(Guid.NewGuid()), false, extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    #endregion

    private void InitializeRealLocalizer()
    {
        var factory = _serviceProvider.GetRequiredService<IStringLocalizerFactory>();
        CouponDomainResourceService.Initialize(factory);
    }

}