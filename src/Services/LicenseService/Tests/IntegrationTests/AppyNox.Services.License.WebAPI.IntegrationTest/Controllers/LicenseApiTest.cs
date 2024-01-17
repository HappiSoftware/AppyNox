using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.WebAPI.IntegrationTest.Fixtures;
using AutoWrapper.Server;
using AutoWrapper.Wrappers;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;

namespace AppyNox.Services.License.WebAPI.IntegrationTest.Controllers
{
    public class LicenseApiTest(LicenseApiTestFixture licenseApiTestFixture) : IClassFixture<LicenseApiTestFixture>
    {
        #region [ Fields ]

        private readonly JsonSerializerOptions _jsonSerializerOptions = licenseApiTestFixture.JsonSerializerOptions;

        private readonly LicenseApiTestFixture _licenseApiTestFixture = licenseApiTestFixture;

        private readonly HttpClient _client = licenseApiTestFixture.Client;

        private readonly ServiceURIs _serviceURIs = licenseApiTestFixture.ServiceURIs;

        #endregion

        #region [ Public Methods ]

        [Fact]
        [Order(1)]
        public async Task GetAll_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync($"{_serviceURIs.LicenseServiceURI}/licenses");

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk();

            // Deserialize the result (assuming apiResponse.Result is a JSON array of LicenseSimpleDto)
            var licenses = apiResponse?.Result is not null
                ? JsonSerializer.Deserialize<IList<LicenseSimpleDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions)
                : null;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(licenses);
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
            var id = license.Id;
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/licenses/{id}";

            // Act
            var response = await _client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var licenseObj = JsonSerializer.Deserialize<LicenseSimpleDto>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(licenseObj);

            #endregion
        }

        [Fact]
        [Order(3)]
        public async Task Create_ShouldAddNewLicense()
        {
            #region [ Create License ]

            // Arrange
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/licenses";
            var uniqueCode = "ffff2";
            var requestBody = new
            {
                code = uniqueCode,
                description = "string",
                licenseKey = "4547a28a-ce2c-4d84-b791-466d7aedd2bb",
                expirationDate = "2025-01-03T20:30:02.928Z",
                maxUsers = 2,
                maxMacAddresses = 3,
                productId = "9991492a-118c-4f20-ac8c-76410d57957c"
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(requestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            #endregion

            #region [ Get Licenses ]

            // Act
            var license = _licenseApiTestFixture.DbContext.Licenses.SingleOrDefault(x => x.Code == uniqueCode);

            // Assert
            Assert.NotNull(license);

            #endregion
        }

        [Fact]
        [Order(4)]
        public async Task Update_ShouldUpdateEntity()
        {
            #region [ Get License ]

            // Arrange
            var license = _licenseApiTestFixture.DbContext.Licenses.FirstOrDefault();

            // Assert
            Assert.NotNull(license);

            #endregion

            #region [ Update License ]

            // Arrange
            Guid id = license.Id;
            DateTime newExpirationDate = license.ExpirationDate.AddDays(5);
            string newLicenseKey = Guid.NewGuid().ToString();
            string newDescription = "new description";
            int newMaxUsers = license.MaxUsers + 1;
            int newmaxMacAddresses = license.MaxMacAddresses + 1;

            string requestUri = $"{_serviceURIs.LicenseServiceURI}/licenses/{id}";
            var requestBody = new
            {
                code = license.Code,
                expirationDate = newExpirationDate,
                licenseKey = newLicenseKey,
                description = newDescription,
                maxUsers = newMaxUsers,
                maxMacAddresses = newmaxMacAddresses,
                productId = license.ProductId,
                id = license.Id
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(requestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get License ]

            if (license != null)
            {
                _licenseApiTestFixture.DbContext.Entry(license).Reload();
            }

            // Assert
            Assert.NotNull(license);
            Assert.Equal(newExpirationDate, license.ExpirationDate);
            Assert.Equal(newLicenseKey, license.LicenseKey);
            Assert.Equal(newDescription, license.Description);
            Assert.Equal(newMaxUsers, license.MaxUsers);
            Assert.Equal(newmaxMacAddresses, license.MaxMacAddresses);

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
            var id = license.Id;
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/licenses/{id}";

            // Act
            var response = await _client.DeleteAsync(requestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get License ]

            // Act
            var getLicense = _licenseApiTestFixture.DbContext.Licenses.SingleOrDefault(x => x.Id == id);

            // Assert
            Assert.Null(getLicense);

            #endregion
        }

        #endregion
    }
}