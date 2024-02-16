using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.GenericCQRSFixtures;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Text.Json;

namespace AppyNox.Services.Coupon.Application.UnitTest.CQRSTests
{
    public class GenericCQRSUnitTest : IClassFixture<GenericCQRSFixture<CouponEntity, CouponId>>
    {
        #region [ Fields ]

        private readonly GenericCQRSFixture<CouponEntity, CouponId> _fixture;

        #endregion

        #region [ Public Constructors ]

        public GenericCQRSUnitTest(GenericCQRSFixture<CouponEntity, CouponId> fixture)
        {
            _fixture = fixture;
            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(CouponEntity), CommonDtoLevelEnums.Simple.GetDisplayName()))
                .Returns(typeof(CouponSimpleDto));

            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(CouponEntity), CouponCreateDetailLevel.Simple.GetDisplayName()))
                .Returns(typeof(CouponSimpleCreateDto));

            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(CouponEntity), CouponUpdateDetailLevel.Simple.GetDisplayName()))
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
        }

        #endregion

        #region [ Public Methods ]

        [Fact]
        public async Task GetAllEntitiesQuery_ShouldSuccess()
        {
            // Act
            var result = await _fixture.MockMediator.Object.Send(new GetAllEntitiesQuery<CouponEntity>(_fixture.MockQueryParameters.Object));

            // Assert
            Assert.NotNull(result);
            Assert.True(result is PaginatedList);
        }

        [Fact]
        public async void GetEntityByIdQuery_ShouldSuccess()
        {
            // Act
            var result = await _fixture.MockMediator.Object.Send(new GetEntityByIdQuery<CouponEntity, CouponId>(It.IsAny<CouponId>(), _fixture.MockQueryParameters.Object));

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
                .Send(new CreateEntityCommand<CouponEntity>(root, CouponCreateDetailLevel.Simple.GetDisplayName()));

            // Assert
            Assert.NotNull(result.basicDto);
            Assert.True(result.basicDto is CouponSimpleCreateDto);
        }

        [Fact]
        public void UpdateEntityCommand_ShouldSuccess()
        {
            // Prepare
            string jsonData = @"{
            }";
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement root = jsonDocument.RootElement;
            Guid id = Guid.NewGuid();
            NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

            // Act
            var result = _fixture.MockMediator.Object
                .Send(new UpdateEntityCommand<CouponEntity, CouponId>(new CouponId(id), root, CouponCreateDetailLevel.Simple.GetDisplayName()));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void DeleteEntityCommand_ShouldSuccess()
        {
            // Act
            var result = _fixture.MockMediator.Object
                .Send(new DeleteEntityCommand<CouponEntity>(new CouponEntity()));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        #endregion
    }
}