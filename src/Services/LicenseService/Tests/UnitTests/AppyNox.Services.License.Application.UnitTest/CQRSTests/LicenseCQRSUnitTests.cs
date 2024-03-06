using AppyNox.Services.Base.Application.Constants;
using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Text.Json;

namespace AppyNox.Services.License.Application.UnitTest.CQRSTests
{
    public class LicenseCQRSUnitTests : IClassFixture<NoxCQRSFixture<LicenseEntity, LicenseId>>
    {
        #region [ Fields ]

        private readonly NoxCQRSFixture<LicenseEntity, LicenseId> _fixture;

        #endregion

        #region [ Public Constructors ]

        public LicenseCQRSUnitTests(NoxCQRSFixture<LicenseEntity, LicenseId> fixture)
        {
            _fixture = fixture;
            _fixture.MockDtoMappingRegistry.Setup(registry => registry.GetDtoType(DtoLevelMappingTypes.DataAccess, typeof(LicenseEntity), CommonDetailLevels.Simple))
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

            #region [ Repository Mocks ]

            LicenseEntity licenseEntity = LicenseEntity.Create
            (
                "LK000",
                "DescriptionLicense",
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddDays(2),
                1,
                2,
                Guid.NewGuid(),
                new ProductId(Guid.NewGuid())
            );

            LicenseSimpleDto licenseSimpleDto = new()
            {
                Code = "code",
                Description = "description",
                LicenseKey = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow.AddDays(2),
                MaxUsers = 2,
                MaxMacAddresses = 2,
                ProductId = new ProductIdDto() { Value = Guid.NewGuid() }
            };

            _fixture.MockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<Type>(), It.IsAny<ICacheService>()))
                .ReturnsAsync(new Mock<PaginatedList>().Object);

            _fixture.MockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<LicenseId>(), It.IsAny<Type>()))
                .ReturnsAsync(licenseSimpleDto);

            _fixture.MockRepository.Setup(repo => repo.AddAsync(It.IsAny<LicenseEntity>()))
                .ReturnsAsync(licenseEntity);

            #endregion

            _fixture.MockMapper.Setup(mapper => mapper.Map(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<Type>()))
                .Returns((object source, Type sourceType, Type destinationType) =>
                {
                    if (destinationType == typeof(LicenseEntity))
                        return licenseEntity;
                    return Activator.CreateInstance(destinationType)!;
                });
        }

        #endregion

        #region [ Public Methods ]

        [Fact]
        public async void GetAllEntitiesQuery_ShouldSuccess()
        {
            // Act
            var result = await _fixture.MockMediator.Object.Send(new GetAllNoxEntitiesQuery<LicenseEntity>(_fixture.MockQueryParameters.Object));

            // Assert
            Assert.NotNull(result);
            Assert.True(result is PaginatedList);
        }

        [Fact]
        public async void GetEntityByIdQuery_ShouldSuccess()
        {
            // Act
            var result = await _fixture.MockMediator.Object.Send(new GetNoxEntityByIdQuery<LicenseEntity, LicenseId>(It.IsAny<LicenseId>(), _fixture.MockQueryParameters.Object));

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
            NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

            // Act
            var result = await _fixture.MockMediator.Object
                .Send(new CreateNoxEntityCommand<LicenseEntity>(root, LicenseCreateDetailLevel.Simple.GetDisplayName()));

            // Assert
            Assert.True(Guid.TryParse(result.ToString(), out _));
        }

        [Fact]
        public void DeleteEntityCommand_ShouldSuccess()
        {
            // Act
            var result = _fixture.MockMediator.Object
                .Send(new DeleteNoxEntityCommand<LicenseEntity, LicenseId>(new LicenseId(Guid.NewGuid())));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        #endregion
    }
}