using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
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
        CouponSimpleDto couponSimpleDto = new()
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
    }

    #endregion

    #region [ Public Methods ]

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldSuccess()
    {
        // Act
        var result = await _mediator.Send(new GetAllNoxEntitiesQuery<CouponAggregate>(_fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetAllNoxEntitiesQuery<CouponAggregate>(_fixture.MockQueryParameters.Object, extensions));

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
        var exception = await Assert.ThrowsAsync<NoxApplicationException>(() => _mediator.Send(new GetAllNoxEntitiesQuery<CouponAggregate>(_fixture.MockQueryParameters.Object, extensions)));

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
        var result = await _mediator.Send(new GetNoxEntityByIdQuery<CouponAggregate, CouponId>(It.IsAny<CouponId>(), _fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is CouponSimpleDto);
    }

    [Fact]
    public async Task GetEntityByIdQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetNoxEntityByIdQuery<CouponAggregate, CouponId>(It.IsAny<CouponId>(), _fixture.MockQueryParameters.Object, Extensions: extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is CouponSimpleDto);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldSuccess()
    {
        // Prepare
        string jsonData = @"
        {
          ""code"": ""fffj9"",
          ""amount"":{
            ""discountAmount"": 2,
            ""minAmount"": 13
          },
          ""description"": ""string"",
          ""couponDetail"" : {
            ""detail"" : ""details detail"",
            ""code"":""deta1"",
            ""couponDetailTags"": [
                {
                    ""tag"" : ""tag1""
                },
                {
                    ""tag"":""tag2""
                }
            ]
          }
        }";
        JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
        JsonElement root = jsonDocument.RootElement;
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator
            .Send<Guid>(new CreateNoxEntityCommand<CouponAggregate>(root, CouponCreateDetailLevel.Bulk.GetDisplayName()));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Prepare
        string jsonData = @"
        {
          ""code"": ""fffj9"",
          ""amount"":{
            ""discountAmount"": 2,
            ""minAmount"": 13
          },
          ""description"": ""string"",
          ""couponDetail"" : {
            ""detail"" : ""details detail"",
            ""code"":""deta1"",
            ""couponDetailTags"": [
                {
                    ""tag"" : ""tag1""
                },
                {
                    ""tag"":""tag2""
                }
            ]
          }
        }";
        JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
        JsonElement root = jsonDocument.RootElement;
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator
            .Send<Guid>(new CreateNoxEntityCommand<CouponAggregate>(root, CouponCreateDetailLevel.Bulk.GetDisplayName(), extensions));

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
}