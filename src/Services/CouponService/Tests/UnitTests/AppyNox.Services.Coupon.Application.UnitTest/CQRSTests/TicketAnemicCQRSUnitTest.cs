using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text.Json;
using NCEHelper = AppyNox.Services.Base.Application.UnitTests.Helpers.NoxCommandExtensionsHelper;

namespace AppyNox.Services.Coupon.Application.UnitTest.CQRSTests;

public class TicketAnemicCQRSUnitTest : IClassFixture<GenericCQRSFixture<Ticket>>
{
    #region [ Fields ]

    private readonly GenericCQRSFixture<Ticket> _fixture;

    private readonly IServiceProvider _serviceProvider;

    private readonly IMediator _mediator;

    #endregion

    #region [ Public Constructors ]

    public TicketAnemicCQRSUnitTest(GenericCQRSFixture<Ticket> fixture)
    {
        _fixture = fixture;

        NoxApplicationLoggerStub logger = new();
        if (!_fixture.DIInitialized)
        {
            _fixture.ServiceCollection.AddCouponApplication(logger);
            _fixture.ServiceCollection.AddScoped(typeof(IGenericRepository<Ticket>), _ => _fixture.MockRepository.Object);
            _fixture.DIInitialized = true;
        }

        _serviceProvider = _fixture.ServiceCollection.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();

        #region [ Repository Mocks ]

        TicketTag mockTag = new();
        Ticket mockTicket = new();
        TicketSimpleDto mockTicketSimpleDto = new();

        _fixture.MockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<ICacheService>()))
            .ReturnsAsync(new Mock<PaginatedList<Ticket>>().Object);

        _fixture.MockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), false, false))
            .ReturnsAsync(mockTicket);

        _fixture.MockRepository.Setup(repo => repo.AddAsync(It.IsAny<Ticket>()))
            .ReturnsAsync(mockTicket);

        #endregion
    }

    #endregion

    #region [ Public Methods ]

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldSuccess()
    {
        // Act
        var result = await _mediator.Send(new GetAllEntitiesQuery<Ticket>(_fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetAllEntitiesQuery<Ticket>(_fixture.MockQueryParameters.Object, extensions));

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
        var result = await _mediator.Send(new GetEntityByIdQuery<Ticket>(It.IsAny<Guid>(), _fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is TicketSimpleDto);
    }
    [Fact]
    public async Task GetEntityByIdQuery_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        // Act
        var result = await _mediator.Send(new GetEntityByIdQuery<Ticket>(It.IsAny<Guid>(), _fixture.MockQueryParameters.Object, Extensions: extensions));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is TicketSimpleDto);
        Assert.Equal(RunStatus.Finished, extensions.Actions[0].Status);
        Assert.Equal(RunStatus.ExitedWithError, extensions.Actions[1].Status);
        Assert.Equal("B0MA0", string.Concat(extensions.RunOrderHistory));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldSuccess()
    {
        // Prepare
        string jsonData = @"{
            ""title"":""Title nwe2"",
            ""content"": ""Ticket content new2"",
            ""reportDate"": ""2024-06-16T11:36:05.3303319Z""
        }";
        JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
        JsonElement root = jsonDocument.RootElement;
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator
            .Send<Guid>(new CreateEntityCommand<Ticket>(root, CommonDetailLevels.Simple));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldCallActions()
    {
        // Prepare
        NoxCommandExtensions extensions = new([new(NCEHelper.DummyMethod1, RunType.Before), new(NCEHelper.DummyMethod2, RunType.After, suspendOnFailure: false)]);

        string jsonData = @"{
            ""title"":""Title nwe2"",
            ""content"": ""Ticket content new2"",
            ""reportDate"": ""2024-06-16T11:36:05.3303319Z""
        }";
        JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
        JsonElement root = jsonDocument.RootElement;
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _mediator
            .Send<Guid>(new CreateEntityCommand<Ticket>(root, CommonDetailLevels.Simple, extensions));

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
        var updateBody = new
        {
            Id = id,
            Title = "t1",
            Content = "c1"
        };

        string jsonString = JsonSerializer.Serialize(updateBody);
        JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
        JsonElement root = jsonDocument.RootElement;

        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = _mediator
            .Send(new UpdateEntityCommand<Ticket>(id, root, CommonDetailLevels.Simple));

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
        var updateBody = new
        {
            Id = id,
            Title = "t1",
            Content = "c1"
        };

        string jsonString = JsonSerializer.Serialize(updateBody);
        JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
        JsonElement root = jsonDocument.RootElement;

        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = _mediator
            .Send(new UpdateEntityCommand<Ticket>(id, root, CommonDetailLevels.Simple, extensions));

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