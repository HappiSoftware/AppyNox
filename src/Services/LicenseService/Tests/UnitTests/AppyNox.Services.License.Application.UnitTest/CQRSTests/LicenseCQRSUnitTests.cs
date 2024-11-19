using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text.Json;

namespace AppyNox.Services.License.Application.UnitTest.CQRSTests
{
    public class LicenseCQRSUnitTests : IClassFixture<NoxApplicationTestFixture>
    {
        #region [ Fields ]

        private readonly NoxApplicationTestFixture _fixture;

        private readonly IServiceProvider _serviceProvider;

        private readonly IMediator _mediator;

        #endregion

        #region [ Public Constructors ]

        public LicenseCQRSUnitTests(NoxApplicationTestFixture fixture)
        {
            _fixture = fixture;

            NoxApplicationLoggerStub<LicenseCQRSUnitTests> logger = new();
            Mock<INoxRepository<LicenseEntity>> mockRepository = new();
            if (!_fixture.DIInitialized)
            {
                var inMemorySettings = new Dictionary<string, string>
                {
                    {"Consul:ServiceName", "TestService"},
                };

                IConfiguration configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(inMemorySettings!)
                    .Build();
                _fixture.Builder.AddLicenseApplication(configuration, logger);
                _fixture.ServiceCollection.AddScoped(typeof(INoxRepository<LicenseEntity>), _ => mockRepository.Object);
                _fixture.DIInitialized = true;
            }

            _serviceProvider = _fixture.ServiceCollection.BuildServiceProvider();
            _mediator = _serviceProvider.GetRequiredService<IMediator>();

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

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<ICacheService>()))
                .ReturnsAsync(new Mock<PaginatedList<LicenseEntity>>().Object);

            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<LicenseId>(), false, false))
                .ReturnsAsync(licenseEntity);

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<LicenseEntity>()))
                .ReturnsAsync(licenseEntity);

            #endregion
        }

        #endregion

        #region [ Public Methods ]

        [Fact]
        public async Task GetAllEntitiesQuery_ShouldSuccess()
        {
            // Act
            var result = await _mediator.Send(new GetAllNoxEntitiesQuery<LicenseEntity>(_fixture.MockQueryParameters.Object));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEntityByIdQuery_ShouldSuccess()
        {
            // Act
            var result = await _mediator.Send(new GetNoxEntityByIdQuery<LicenseEntity, LicenseId>(It.IsAny<LicenseId>(), _fixture.MockQueryParameters.Object));

            // Assert
            Assert.NotNull(result);
            Assert.True(result is LicenseSimpleDto);
        }

        [Fact]
        public async Task CreateEntityCommand_ShouldSuccess()
        {
            // Prepare
            string jsonData = @"{
              ""code"": ""LK002"",
              ""expirationDate"": ""2025-01-03T20:30:02.928Z"",
              ""licenseKey"": ""00b7fb79-9a1c-4786-abb9-1ea9bd7f379esssssssssssssss"",
              ""description"": ""new description"",
              ""maxUsers"" : 4,
              ""maxMacAddresses"" : 3,
              ""productId"" : {
                ""value"" : ""9991492a-118c-4f20-ac8c-76410d57957c""
              }
            }";
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement root = jsonDocument.RootElement;
            NoxContext.UserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");

            // Act
            var result = await _mediator
                .Send(new CreateNoxEntityCommand<LicenseEntity>(root, LicenseCreateDetailLevel.Simple.GetDisplayName()));

            // Assert
            Assert.True(Guid.TryParse(result.ToString(), out _));
        }

        [Fact]
        public void DeleteEntityCommand_ShouldSuccess()
        {
            // Act
            var result = _mediator
                .Send(new DeleteNoxEntityCommand<LicenseEntity, LicenseId>(new LicenseId(Guid.NewGuid()), false));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }

        #endregion
    }
}