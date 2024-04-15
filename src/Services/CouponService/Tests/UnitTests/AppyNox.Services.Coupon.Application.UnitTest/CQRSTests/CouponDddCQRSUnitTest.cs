using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AppyNox.Services.Coupon.Domain.Localization;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Moq;
using System.Reflection;
using System.Text.Json;
using CouponAggregate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Application.UnitTest.CQRSTests;

public class CouponDddCQRSUnitTest : IClassFixture<NoxCQRSFixture<Domain.Coupons.Coupon, CouponId>>
{
    #region [ Fields ]

    private readonly NoxCQRSFixture<Domain.Coupons.Coupon, CouponId> _fixture;

    #endregion

    #region [ Public Constructors ]

    public CouponDddCQRSUnitTest(NoxCQRSFixture<Domain.Coupons.Coupon, CouponId> fixture)
    {
        var localizer = new Mock<IStringLocalizer>();
        localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

        var localizerFactory = new Mock<IStringLocalizerFactory>();
        localizerFactory.Setup(lf => lf.Create(typeof(CouponDomainResourceService))).Returns(localizer.Object);

        CouponDomainResourceService.Initialize(localizerFactory.Object);

        _fixture = fixture;
        _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(CouponAggregate), CommonDetailLevels.Simple))
            .Returns(typeof(CouponSimpleDto));

        _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(CouponAggregate), CouponCreateDetailLevel.Bulk.GetDisplayName()))
            .Returns(typeof(CouponBulkCreateDto));

        _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(CouponAggregate), CouponUpdateDetailLevel.Simple.GetDisplayName()))
            .Returns(typeof(CouponSimpleUpdateDto));

        var validatorMock = new Mock<IValidator<CouponBulkCreateDto>>();
        validatorMock
            .Setup(validator => validator.Validate(It.IsAny<ValidationContext<object>>()))
            .Returns(new ValidationResult());

        _fixture.MockServiceProvider.Setup(service => service.GetService(typeof(IValidator<CouponBulkCreateDto>)))
            .Returns(validatorMock.Object);

        var validatorMockUpdate = new Mock<IValidator<CouponSimpleUpdateDto>>();
        validatorMockUpdate
            .Setup(validator => validator.Validate(It.IsAny<ValidationContext<object>>()))
            .Returns(new ValidationResult());
        _fixture.MockServiceProvider.Setup(service => service.GetService(typeof(IValidator<CouponSimpleUpdateDto>)))
            .Returns(validatorMockUpdate.Object);

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
        _fixture.MockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<ICacheService>()))
            .ReturnsAsync(new Mock<PaginatedList<CouponAggregate>>().Object);

        _fixture.MockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<CouponId>()))
            .ReturnsAsync(couponEntity);

        _fixture.MockRepository.Setup(repo => repo.AddAsync(It.IsAny<CouponAggregate>()))
            .ReturnsAsync(couponEntity);

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
        var result = await _fixture.MockMediator.Object.Send(new GetAllNoxEntitiesQuery<Domain.Coupons.Coupon>(_fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetEntityByIdQuery_ShouldSuccess()
    {
        // Act
        var result = await _fixture.MockMediator.Object.Send(new GetNoxEntityByIdQuery<Domain.Coupons.Coupon, CouponId>(It.IsAny<CouponId>(), _fixture.MockQueryParameters.Object));

        // Assert
        Assert.NotNull(result);
        Assert.True(result is CouponSimpleDto);
    }

    [Fact]
    public async void CreateEntityCommand_ShouldSuccess()
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
        var result = await _fixture.MockMediator.Object
            .Send<Guid>(new CreateNoxEntityCommand<Domain.Coupons.Coupon>(root, CouponCreateDetailLevel.Bulk.GetDisplayName()));

        // Assert
        Assert.True(Guid.TryParse(result.ToString(), out _));
    }

    [Fact]
    public void DeleteEntityCommand_ShouldSuccess()
    {
        // Act
        var result = _fixture.MockMediator.Object
            .Send(new DeleteNoxEntityCommand<CouponAggregate, CouponId>(new CouponId(Guid.NewGuid())));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompletedSuccessfully);
    }

    #endregion
}