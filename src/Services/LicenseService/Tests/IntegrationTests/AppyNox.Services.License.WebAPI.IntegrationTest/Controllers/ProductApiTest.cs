using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models.Base;
using AppyNox.Services.License.WebAPI.IntegrationTest.Fixtures;
using AutoWrapper.Server;
using AutoWrapper.Wrappers;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;

namespace AppyNox.Services.License.WebAPI.IntegrationTest.Controllers
{
    public class ProductApiTest(LicenseApiTestFixture licenseApiTestFixture) : IClassFixture<LicenseApiTestFixture>
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
            var response = await _client.GetAsync($"{_serviceURIs.LicenseServiceURI}/products");

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk();

            // Deserialize the result (assuming apiResponse.Result is a JSON array of ProductSimpleDto)
            var products = apiResponse?.Result is not null
                ? JsonSerializer.Deserialize<IList<ProductSimpleDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions)
                : null;

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
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/products/{id}";

            // Act
            var response = await _client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var productObj = JsonSerializer.Deserialize<ProductSimpleDto>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

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
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/products";
            var requestBody = new
            {
                name = "NewProduct"
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(requestUri, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            var result = JsonSerializer.Deserialize<(Guid, ProductSimpleDto)>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            #endregion

            #region [ Get Products ]

            // Act
            var product = _licenseApiTestFixture.DbContext.Products.SingleOrDefault(x => x.Id == result.Item1);

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

            string requestUri = $"{_serviceURIs.LicenseServiceURI}/products/{id}";
            var requestBody = new
            {
                name = newName,
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
            var requestUri = $"{_serviceURIs.LicenseServiceURI}/products/{id}";

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