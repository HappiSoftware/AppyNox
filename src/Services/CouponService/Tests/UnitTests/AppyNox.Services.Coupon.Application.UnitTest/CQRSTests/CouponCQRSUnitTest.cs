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
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Moq;
using System.Text.Json;
using CouponAggregate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Application.UnitTest.CQRSTests
{
    public class CouponCQRSUnitTest : IClassFixture<NoxCQRSFixture<Domain.Coupons.Coupon, CouponId>>
    {
        #region [ Fields ]

        private readonly NoxCQRSFixture<Domain.Coupons.Coupon, CouponId> _fixture;

        #endregion

        #region [ Public Constructors ]

        public CouponCQRSUnitTest(NoxCQRSFixture<Domain.Coupons.Coupon, CouponId> fixture)
        {
            var localizer = new Mock<IStringLocalizer>();
            localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

            var localizerFactory = new Mock<IStringLocalizerFactory>();
            localizerFactory.Setup(lf => lf.Create(typeof(CouponDomainResourceService))).Returns(localizer.Object);

            CouponDomainResourceService.Initialize(localizerFactory.Object);

            _fixture = fixture;
            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(CouponAggregate), CommonDetailLevels.Simple))
                .Returns(typeof(CouponSimpleDto));

            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(CouponAggregate), CouponCreateDetailLevel.Simple.GetDisplayName()))
                .Returns(typeof(CouponSimpleCreateDto));

            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(CouponAggregate), CouponUpdateDetailLevel.Simple.GetDisplayName()))
                .Returns(typeof(CouponSimpleUpdateDto));

            var validatorMock = new Mock<IValidator<CouponSimpleCreateDto>>();
            validatorMock
                .Setup(validator => validator.Validate(It.IsAny<ValidationContext<object>>()))
                .Returns(new ValidationResult());
            _fixture.MockServiceProvider.Setup(service => service.GetService(typeof(IValidator<CouponSimpleCreateDto>)))
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
            _fixture.MockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<Type>(), It.IsAny<ICacheService>()))
                .ReturnsAsync(new Mock<PaginatedList>().Object);

            _fixture.MockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<CouponId>(), It.IsAny<Type>()))
                .ReturnsAsync(couponSimpleDto);

            _fixture.MockRepository.Setup(repo => repo.AddAsync(It.IsAny<CouponAggregate>()))
                .ReturnsAsync(couponEntity);

            _fixture.MockMapper.Setup(mapper => mapper.Map(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<Type>()))
                .Returns((object source, Type sourceType, Type destinationType) =>
                {
                    if (destinationType == typeof(CouponAggregate))
                        return couponEntity;
                    return Activator.CreateInstance(destinationType)!;
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
            Assert.True(result is PaginatedList);
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
            string jsonData = @"{
            }";
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement root = jsonDocument.RootElement;
            NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

            // Act
            var result = await _fixture.MockMediator.Object
                .Send<(Guid guid, CouponAggregate entity)>(new CreateNoxEntityCommand<Domain.Coupons.Coupon>(root, CouponCreateDetailLevel.Simple.GetDisplayName()));

            // Assert
            Assert.NotNull(result.entity);
            Assert.True(result.entity is CouponAggregate);
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
}