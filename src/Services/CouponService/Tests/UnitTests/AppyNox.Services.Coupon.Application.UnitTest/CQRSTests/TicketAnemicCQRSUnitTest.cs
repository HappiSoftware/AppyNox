using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;
using AppyNox.Services.Coupon.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text.Json;
using NCEHelper = AppyNox.Services.Base.Application.UnitTests.Helpers.NoxCommandExtensionsHelper;

namespace AppyNox.Services.Coupon.Application.UnitTest.CQRSTests;

public class TicketAnemicCQRSUnitTest : IClassFixture<NoxApplicationTestFixture>
{
    #region [ Fields ]

    private readonly NoxApplicationTestFixture _fixture;

    private readonly IServiceProvider _serviceProvider;

    private readonly IMediator _mediator;

    #endregion

    #region [ Public Constructors ]

    public TicketAnemicCQRSUnitTest(NoxApplicationTestFixture fixture)
    {
        _fixture = fixture;

        NoxApplicationLoggerStub<TicketAnemicCQRSUnitTest> logger = new();
        Mock<IGenericRepository<Ticket>> mockRepository = new();
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
            _fixture.ServiceCollection.AddScoped(typeof(IGenericRepository<Ticket>), _ => mockRepository.Object);
            _fixture.DIInitialized = true;
        }

        _serviceProvider = _fixture.ServiceCollection.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();

        #region [ Repository Mocks ]

        TicketTag mockTag = new();
        Ticket mockTicket = new();
        TicketDto mockTicketSimpleDto = new();

        mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<ICacheService>()))
            .ReturnsAsync(new Mock<PaginatedList<Ticket>>().Object);

        mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), false, false))
            .ReturnsAsync(mockTicket);

        mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Ticket>()))
            .ReturnsAsync(mockTicket);

        #endregion
    }

    #endregion

    #region [ Public Methods ]

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldSuccess()
    {
        // Act
        var result = await _mediator.Send(new GetAllEntitiesQuery<Ticket, TicketDto>(_fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetAllEntitiesQuery<Ticket, TicketDto>(_fixture.MockQueryParameters.Object, extensions));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task GetEntityByIdQuery_ShouldSuccess()
    {
        // Act
        var result = await _mediator.Send(new GetEntityByIdQuery<Ticket, TicketDto>(It.IsAny<Guid>(), _fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is TicketDto);
    }
    [Fact]
    public async Task GetEntityByIdQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetEntityByIdQuery<Ticket, TicketDto>(It.IsAny<Guid>(), _fixture.MockQueryParameters.Object, Extensions: extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is TicketDto);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldSuccess()
    {
        // Prepare
        TicketCreateDto dto = new()
        {
            Title = "Title new",
            Content = "Ticket content new",
            ReportDate = DateTime.Now,
        };
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator.Send(new CreateEntityCommand<Ticket, TicketCreateDto>(dto));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        TicketCreateDto dto = new()
        {
            Title = "Title new",
            Content = "Ticket content new",
            ReportDate = DateTime.Now,
        };
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator.Send(new CreateEntityCommand<Ticket, TicketCreateDto>(dto, extensions));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public void UpdateEntityCommand_ShouldSuccess()
    {
        // Prepare
        Guid id = new();
        TicketUpdateDto dto = new()
        {
            Id = id,
            Title = "t1",
            Content = "c1"
        };

        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = _mediator
            .Send(new UpdateEntityCommand<Ticket, TicketUpdateDto>(id, dto));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
    }

    [Fact]
    public void UpdateEntityCommand_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        Guid id = new();
        TicketUpdateDto dto = new()
        {
            Id = id,
            Title = "t1",
            Content = "c1"
        };

        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = _mediator
            .Send(new UpdateEntityCommand<Ticket, TicketUpdateDto>(id, dto, extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public void DeleteEntityCommand_ShouldSuccess()
    {
        // Act
        var result = _mediator
            .Send(new DeleteEntityCommand<Ticket>(new Guid()));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
    }

    [Fact]
    public void DeleteEntityCommand_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = _mediator
            .Send(new DeleteEntityCommand<Ticket>(new Guid(), Extensions: extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    #endregion
}