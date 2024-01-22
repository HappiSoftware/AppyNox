using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Wrappers.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.WebAPI.IntegrationTest.Fixtures;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;

namespace AppyNox.Services.License.WebAPI.IntegrationTest.Controllers
{
    [Collection("LicenseService Collection")]
    public class ProductApiTest(LicenseServiceFixture licenseApiTestFixture)
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
            var response = await _client.GetAsync($"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/products");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var products = NoxResponseUnwrapper.UnwrapData<IList<ProductSimpleDto>>(jsonResponse, jsonSerializerOptions: _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(products);
        }

        [Fact]
        [Order(2)]
        public async Task GetById_ShouldReturnSuccessStatusCode()
        {
            #region [ Get Product ]

            // Arrange
            var product = _licenseApiTestFixture.DbContext.Products.FirstOrDefault();

            // Assert
            Assert.NotNull(product);

            #endregion

            #region [ Get Product By Id ]

            // Arrange
            var id = product.Id;
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/products/{id}";

            // Act
            var response = await _client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var productObj = NoxResponseUnwrapper.UnwrapData<ProductSimpleDto>(jsonResponse, jsonSerializerOptions: _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(productObj);

            #endregion
        }

        [Fact]
        [Order(3)]
        public async Task Create_ShouldAddNewProduct()
        {
            #region [ Create Product ]

            // Arrange
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/products";
            var requestBody = new
            {
                name = "NewProduct",
                code = "drop1"
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _client.PostAsync(requestUri, content);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            (Guid id, ProductSimpleDto createdObject) = NoxResponseUnwrapper.UnwrapDataWithId<ProductSimpleDto>(jsonResponse, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(createdObject);

            #endregion

            #region [ Get Products ]

            // Act
            var product = _licenseApiTestFixture.DbContext.Products.SingleOrDefault(x => x.Id == id);

            // Assert
            Assert.NotNull(product);

            #endregion
        }

        [Fact]
        [Order(4)]
        public async Task Update_ShouldUpdateEntity()
        {
            #region [ Get Product ]

            // Arrange
            var product = _licenseApiTestFixture.DbContext.Products.FirstOrDefault();

            // Assert
            Assert.NotNull(product);

            #endregion

            #region [ Update Product ]

            // Arrange
            Guid id = product.Id;
            string newName = "NameUpdated";
            string newCode = "drop2";

            string requestUri = $"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/products/{id}";
            var requestBody = new
            {
                name = newName,
                code = newCode,
                id = id,
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(requestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get Product ]

            if (product != null)
            {
                _licenseApiTestFixture.DbContext.Entry(product).Reload();
            }

            // Assert
            Assert.NotNull(product);
            Assert.Equal(newName, product.Name);
            Assert.Equal(newCode, product.Code);

            #endregion
        }

        [Fact]
        [Order(5)]
        public async Task Delete_ShouldDeleteEntity()
        {
            #region [ Get Product ]

            // Arrange
            var product = _licenseApiTestFixture.DbContext.Products.FirstOrDefault();

            // Assert
            Assert.NotNull(product);

            #endregion

            #region [ Delete Product ]

            // Arrange
            var id = product.Id;
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/v{NoxVersions.v1_0}/products/{id}";

            // Act
            var response = await _client.DeleteAsync(requestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get Product ]

            // Act
            var getProduct = _licenseApiTestFixture.DbContext.Products.SingleOrDefault(x => x.Id == id);

            // Assert
            Assert.Null(getProduct);

            #endregion
        }

        #endregion
    }
}