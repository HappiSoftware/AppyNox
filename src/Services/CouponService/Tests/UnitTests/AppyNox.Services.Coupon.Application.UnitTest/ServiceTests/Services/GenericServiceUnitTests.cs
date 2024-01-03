using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Application.UnitTests.ServiceTests;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Text.Json;

namespace AppyNox.Services.Coupon.Application.UnitTest.ServiceTests.Services
{
    public class GenericServiceUnitTests : IClassFixture<ServiceFixture<CouponEntity>>
    {
        #region [ Fields ]

        private readonly ServiceFixture<CouponEntity> _fixture;

        #endregion

        #region [ Public Constructors ]

        public GenericServiceUnitTests(ServiceFixture<CouponEntity> fixture)
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
        public async void GetAllAsync_ShouldReturnData()
        {
            // Act
            var result = await _fixture.GenericServiceBase.GetAllAsync(new QueryParameters());

            // Assert
            Assert.NotNull(result);
            Assert.True(result is IEnumerable<dynamic>);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnData()
        {
            // Act
            var result = await _fixture.GenericServiceBase.GetByIdAsync(It.IsAny<Guid>(), new QueryParameters());

            // Assert
            Assert.NotNull(result);
            Assert.True(result is CouponSimpleDto);
        }

        [Fact]
        public async void AddAsync_ShouldReturnData()
        {
            // Prepare
            string jsonData = @"{
                ""code"": ""fffff2"",
                ""discountAmount"": 2,
                ""minAmount"": 12,
                ""description"": ""string"",
                ""couponDetailEntityId"": ""c2feaca4-d82a-4d2e-ba5a-667b685212b4""
            }";
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement root = jsonDocument.RootElement;

            // Act
            var result = await _fixture.GenericServiceBase.AddAsync(root, CouponCreateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result.basicDto);
            Assert.True(result.basicDto is CouponSimpleDto);
        }

        [Fact]
        public void UpdateAsync_ShouldBeSuccessful()
        {
            // Prepare
            string jsonData = @"{
                ""id"":""934753af-ef04-47d5-b44e-debdb0a05658"",
                ""code"": ""fffff2"",
                ""discountAmount"": 2,
                ""minAmount"": 12,
                ""description"": ""string"",
                ""couponDetailEntityId"": ""c2feaca4-d82a-4d2e-ba5a-667b685212b4""
            }";
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement root = jsonDocument.RootElement;

            // Act
            var result = _fixture.GenericServiceBase.UpdateAsync(root, CouponUpdateDetailLevel.Simple.GetDisplayName());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void DeleteAsync_ShouldBeSuccessful()
        {
            // Act
            var result = _fixture.GenericServiceBase.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        #endregion
    }
}