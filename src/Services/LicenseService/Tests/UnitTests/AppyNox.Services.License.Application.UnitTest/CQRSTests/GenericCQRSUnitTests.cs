using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.MediatR.Queries;
using AppyNox.Services.Base.Application.UnitTests.GenericCQRSFixtures;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Text.Json;

namespace AppyNox.Services.License.Application.UnitTest.CQRSTests
{
    public class GenericCQRSUnitTests : IClassFixture<GenericCQRSFixture<LicenseEntity>>
    {
        #region [ Fields ]

        private readonly GenericCQRSFixture<LicenseEntity> _fixture;

        #endregion

        #region [ Public Constructors ]

        public GenericCQRSUnitTests(GenericCQRSFixture<LicenseEntity> fixture)
        {
            _fixture = fixture;
            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(LicenseEntity), CommonDtoLevelEnums.Simple.GetDisplayName()))
                .Returns(typeof(LicenseSimpleDto));

            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Create, typeof(LicenseEntity), LicenseCreateDetailLevel.Simple.GetDisplayName()))
                .Returns(typeof(LicenseSimpleCreateDto));

            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.Update, typeof(LicenseEntity), LicenseUpdateDetailLevel.Simple.GetDisplayName()))
                .Returns(typeof(LicenseSimpleUpdateDto));

            var validatorMock = new Mock<IValidator<LicenseSimpleCreateDto>>();
            validatorMock
                .Setup(validator => validator.Validate(It.IsAny<ValidationContext<object>>()))
                .Returns(new ValidationResult());
            _fixture.MockServiceProvider.Setup(service => service.GetService(typeof(IValidator<LicenseSimpleCreateDto>)))
                .Returns(validatorMock.Object);

            var validatorMockUpdate = new Mock<IValidator<LicenseSimpleUpdateDto>>();
            validatorMockUpdate
                .Setup(validator => validator.Validate(It.IsAny<ValidationContext<object>>()))
                .Returns(new ValidationResult());
            _fixture.MockServiceProvider.Setup(service => service.GetService(typeof(IValidator<LicenseSimpleUpdateDto>)))
                .Returns(validatorMockUpdate.Object);
        }

        #endregion

        #region [ Public Methods ]

        [Fact]
        public async void GetAllEntitiesQuery_ShouldSuccess()
        {
            // Act
            var result = await _fixture.MockMediator.Object.Send(new GetAllEntitiesQuery<LicenseEntity>(_fixture.MockQueryParameters.Object));

            // Assert
            Assert.NotNull(result);
            Assert.True(result is IEnumerable<dynamic>);
        }

        [Fact]
        public async void GetEntityByIdQuery_ShouldSuccess()
        {
            // Act
            var result = await _fixture.MockMediator.Object.Send(new GetEntityByIdQuery<LicenseEntity>(It.IsAny<Guid>(), _fixture.MockQueryParameters.Object));

            // Assert
            Assert.NotNull(result);
            Assert.True(result is LicenseSimpleDto);
        }

        [Fact]
        public async void CreateEntityCommand_ShouldSuccess()
        {
            // Prepare
            string jsonData = @"{
            }";
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement root = jsonDocument.RootElement;

            // Act
            var result = await _fixture.MockMediator.Object
                .Send(new CreateEntityCommand<LicenseEntity>(root, LicenseCreateDetailLevel.Simple.GetDisplayName()));

            // Assert
            Assert.NotNull(result.basicDto);
            Assert.True(result.basicDto is LicenseSimpleDto);
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

            // Act
            var result = _fixture.MockMediator.Object
                .Send(new UpdateEntityCommand<LicenseEntity>(id, root, LicenseCreateDetailLevel.Simple.GetDisplayName()));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void DeleteEntityCommand_ShouldSuccess()
        {
            // Act
            var result = _fixture.MockMediator.Object
                .Send(new DeleteEntityCommand<LicenseEntity>(It.IsAny<Guid>()));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        #endregion
    }
}