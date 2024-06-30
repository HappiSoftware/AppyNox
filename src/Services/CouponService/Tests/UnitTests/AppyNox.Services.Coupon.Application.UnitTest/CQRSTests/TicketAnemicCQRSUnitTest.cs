using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Domain.Localization;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Moq;
using System.Reflection;
using System.Text.Json;

namespace AppyNox.Services.Coupon.Application.UnitTest.CQRSTests;

public class TicketAnemicCQRSUnitTest : IClassFixture<GenericCQRSFixture<Ticket>>
{
    #region [ Fields ]

    private readonly GenericCQRSFixture<Ticket> _fixture;

    #endregion

    #region [ Public Constructors ]

    public TicketAnemicCQRSUnitTest(GenericCQRSFixture<Ticket> fixture)
    {
        var localizer = new Mock<IStringLocalizer>();
        localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

        var localizerFactory = new Mock<IStringLocalizerFactory>();
        localizerFactory.Setup(lf => lf.Create(typeof(CouponDomainResourceService))).Returns(localizer.Object);

        CouponDomainResourceService.Initialize(localizerFactory.Object);

        _fixture = fixture;

        #region [ Dto Mapping Registry ]

        _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(Ticket), CommonDetailLevels.Simple))
            .Returns(typeof(TicketSimpleDto));

        _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(Ticket), CommonDetailLevels.Simple))
            .Returns(typeof(TicketSimpleCreateDto));

        _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(Ticket), CommonDetailLevels.Simple))
            .Returns(typeof(TicketSimpleUpdateDto));

        #endregion

        #region [ Validator ]

        // Create
        var validatorMock = new Mock<IValidator<TicketSimpleCreateDto>>();

        validatorMock
            .Setup(validator => validator.Validate(It.IsAny<ValidationContext<object>>()))
            .Returns(new ValidationResult());

        _fixture.MockServiceProvider.Setup(service => service.GetService(typeof(IValidator<TicketSimpleCreateDto>)))
            .Returns(validatorMock.Object);

        // Update
        var validatorMockUpdate = new Mock<IValidator<TicketSimpleUpdateDto>>();

        validatorMockUpdate
            .Setup(validator => validator.Validate(It.IsAny<ValidationContext<object>>()))
            .Returns(new ValidationResult());

        _fixture.MockServiceProvider.Setup(service => service.GetService(typeof(IValidator<TicketSimpleUpdateDto>)))
            .Returns(validatorMockUpdate.Object);

        #endregion

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

        #region [ Mapper ]

        Assembly applicationAssembly = Assembly.Load("AppyNox.Services.Coupon.Application");
        var profiles = applicationAssembly.GetTypes()
            .Where(t => typeof(Profile).IsAssignableFrom(t))
            .Select(Activator.CreateInstance)
            .Cast<Profile>();

        var configuration = new MapperConfiguration(cfg =>
        {
            foreach (var profile in profiles)
            {
                cfg.AddProfile(profile);
            }
        });

        _fixture.MockMapper.Setup(mapper => mapper.Map(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<Type>()))
            .Returns((object source, Type sourceType, Type destinationType) =>
            {
                var mapper = configuration.CreateMapper();
                return mapper.Map(source, sourceType, destinationType);
            });

        #endregion
    }

    #endregion

    #region [ Public Methods ]

    [Fact]
    public async Task GetAllEntitiesQuery_ShouldSuccess()
    {
        // Act
        var result = await _fixture.MockMediator.Object.Send(new GetAllEntitiesQuery<Ticket>(_fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEntityByIdQuery_ShouldSuccess()
    {
        // Act
        var result = await _fixture.MockMediator.Object.Send(new GetEntityByIdQuery<Ticket>(It.IsAny<Guid>(), _fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is TicketSimpleDto);
    }

    [Fact]
    public async Task CreateEntityCommand_ShouldSuccess()
    {
        // Prepare
        string jsonData = @"{
            }";
        JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
        JsonElement root = jsonDocument.RootElement;
        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = await _fixture.MockMediator.Object
            .Send<Guid>(new CreateEntityCommand<Ticket>(root, CommonDetailLevels.Simple));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
    }

    [Fact]
    public void UpdateEntityCommand_ShouldSuccess()
    {
        // Prepare
        Guid id = new();
        var updateBody = new
        {
            Id = id
        };

        string jsonString = JsonSerializer.Serialize(updateBody);
        JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
        JsonElement root = jsonDocument.RootElement;

        NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

        // Act
        var result = _fixture.MockMediator.Object
            .Send(new UpdateEntityCommand<Ticket>(id, root, CommonDetailLevels.Simple));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
    }

    [Fact]
    public void DeleteEntityCommand_ShouldSuccess()
    {
        // Act
        var result = _fixture.MockMediator.Object
            .Send(new DeleteEntityCommand<Ticket>(new Guid()));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
    }

    #endregion
}