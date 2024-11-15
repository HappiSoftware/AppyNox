using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Base.IntegrationTests.Wrapper.Helpers;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.WebAPI.IntegrationTest.Fixtures;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;

namespace AppyNox.Services.License.WebAPI.IntegrationTest.Controllers
{
    [Collection("LicenseService Collection")]
    public class LicenseApiTest(LicenseServiceFixture licenseApiTestFixture)
    {
        #region [ Fields ]

        private readonly JsonSerializerOptions _jsonSerializerOptions = licenseApiTestFixture.JsonSerializerOptions;

        private readonly LicenseServiceFixture _licenseApiTestFixture = licenseApiTestFixture;

        private readonly HttpClient _client = licenseApiTestFixture.Client;

        private readonly ServiceURIs _serviceURIs = licenseApiTestFixture.ServiceURIs;

        #endregion

        #region [ Public Methods ]

        [Fact]
        [Order(1)]
        public async Task GetAll_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync($"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/licenses");

            var wrappedResponse= await NoxResponseUnwrapper.UnwrapResponse<PaginatedList<LicenseDto>>(response, jsonSerializerOptions: _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(wrappedResponse.Result.Data);
            Assert.NotNull(wrappedResponse.Result.Data.Items);
        }

        [Fact]
        [Order(2)]
        public async Task GetById_ShouldReturnSuccessStatusCode()
        {
            #region [ Get License ]

            // Arrange
            var license = _licenseApiTestFixture.DbContext.Licenses.FirstOrDefault();

            // Assert
            Assert.NotNull(license);

            #endregion

            #region [ Get License By Id ]

            // Arrange
            Guid id = license.Id.Value;
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/licenses/{id}";

            // Act
            var response = await _client.GetAsync(requestUri);
            var noxApiResponse = await NoxResponseUnwrapper.UnwrapResponse<LicenseDto>(response, jsonSerializerOptions: _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(noxApiResponse.Result.Data);

            #endregion
        }

        [Fact]
        [Order(4)]
        public async Task Create_ShouldAddNewLicense()
        {
            #region [ Create License ]

            // Arrange
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/licenses";
            var uniqueCode = "ffff2";
            LicenseCreateDto dto = new()
            {
                Code = uniqueCode,
                Description = "string",
                LicenseKey = "4547a28a-ce2c-4d84-b791-466d7aedd2bb",
                ExpirationDate = DateTime.Now,
                MaxUsers = 2,
                MaxMacAddresses = 3,
                ProductId = new()
                {
                    Value = new Guid("9991492a-118c-4f20-ac8c-76410d57957c")
                }
            };
            var jsonRequest = JsonSerializer.Serialize(new
            {
                dto.Code,
                dto.Description,
                dto.LicenseKey,
                ExpirationDate = dto.ExpirationDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                dto.MaxUsers,
                dto.MaxMacAddresses,
                dto.ProductId
            });
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _client.PostAsync(requestUri, content);
            var jsonData = response.Content.ReadAsStringAsync();
            NoxApiResponse<Guid> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<Guid>(response, _jsonSerializerOptions);
            Guid id = wrappedResponse.Result.Data;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(id, Guid.Empty);

            #endregion

            #region [ Get Licenses ]

            // Act
            var license = _licenseApiTestFixture.DbContext.Licenses.Where("Id == @0", new LicenseId(id)).FirstOrDefault();

            // Assert
            Assert.NotNull(license);

            #endregion
        }

        [Fact]
        [Order(5)]
        public async Task Delete_ShouldDeleteEntity()
        {
            #region [ Get License ]

            // Arrange
            var license = _licenseApiTestFixture.DbContext.Licenses.FirstOrDefault();

            // Assert
            Assert.NotNull(license);

            #endregion

            #region [ Delete License ]

            // Arrange
            Guid id = license.Id.Value;
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/licenses/{id}";

            // Act
            var response = await _client.DeleteAsync(requestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get License ]

            // Act
            var getLicense = _licenseApiTestFixture.DbContext.Licenses.SingleOrDefault(x => x.Id == license.Id);

            // Assert
            Assert.Null(getLicense);

            #endregion
        }

        #endregion
    }
}