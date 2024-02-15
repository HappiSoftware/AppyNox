using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Base.IntegrationTests.Wrapper.Helpers;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.WebAPI.IntegrationTest.Fixtures;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTest
{
    public class CouponApiTest(CouponApiTestFixture couponApiTestFixture) : IClassFixture<CouponApiTestFixture>
    {
        #region [ Fields ]

        private readonly JsonSerializerOptions _jsonSerializerOptions = couponApiTestFixture.JsonSerializerOptions;

        private readonly CouponApiTestFixture _couponApiTestFixture = couponApiTestFixture;

        private readonly HttpClient _client = couponApiTestFixture.Client;

        private readonly ServiceURIs _serviceURIs = couponApiTestFixture.ServiceURIs;

        #endregion

        #region [ Public Methods ]

        [Fact]
        public async Task GetAll_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync($"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var coupons = NoxResponseUnwrapper.UnwrapData<PaginatedList>(jsonResponse, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupons.Items);
        }

        [Fact]
        public async Task GetById_ShouldReturnSuccessStatusCode()
        {
            #region [ Get Coupon ]

            // Arrange
            var coupon = _couponApiTestFixture.DbContext.Coupons.FirstOrDefault();

            // Assert
            Assert.NotNull(coupon);

            #endregion

            #region [ Get Coupon By Id ]

            // Arrange
            var id = coupon.Id;
            var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons/{id}";

            // Act
            var response = await _client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var couponObj = NoxResponseUnwrapper.UnwrapData<CouponSimpleDto>(jsonResponse, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(couponObj);

            #endregion
        }

        [Fact]
        public async Task Create_ShouldAddNewCoupon()
        {
            #region [ Create Coupon ]

            // Arrange
            var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons";
            var uniqueCode = "ffff2";
            var requestBody = new
            {
                code = uniqueCode,
                discountAmount = 2,
                minAmount = 12,
                description = "string",
                couponDetailEntityId = "ec80532f-58f0-4690-b40c-2133b067d5f2"
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _client.PostAsync(requestUri, content);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            (Guid id, CouponSimpleDto createdObject) = NoxResponseUnwrapper.UnwrapDataWithId<CouponSimpleDto>(jsonResponse, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(createdObject);

            #endregion

            #region [ Get Coupons ]

            // Act
            var coupon = _couponApiTestFixture.DbContext.Coupons.SingleOrDefault(x => x.Id == id);

            // Assert
            Assert.NotNull(coupon);

            #endregion
        }

        [Fact]
        public async Task Update_ShouldUpdateEntity()
        {
            #region [ Get Coupon ]

            // Arrange
            var coupon = _couponApiTestFixture.DbContext.Coupons.FirstOrDefault();

            // Assert
            Assert.NotNull(coupon);

            #endregion

            #region [ Update Coupon ]

            // Arrange
            var id = coupon.Id;
            var newDiscountAmount = coupon.DiscountAmount + 1;
            var newMinAmount = coupon.MinAmount + 1;
            var newDescription = "new description";
            var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons/{id}";
            var requestBody = new
            {
                code = coupon.Code,
                discountAmount = newDiscountAmount,
                minAmount = newMinAmount,
                description = newDescription,
                couponDetailEntityId = coupon.CouponDetailEntityId,
                id = coupon.Id
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(requestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get Coupon ]

            if (coupon != null)
            {
                _couponApiTestFixture.DbContext.Entry(coupon).Reload();
            }

            // Assert
            Assert.NotNull(coupon);
            Assert.Equal(newDiscountAmount, coupon.DiscountAmount);
            Assert.Equal(newMinAmount, coupon.MinAmount);
            Assert.Equal(newDescription, coupon.Description);

            #endregion
        }

        [Fact]
        public async Task Delete_ShouldDeleteEntity()
        {
            #region [ Get Coupon ]

            // Arrange
            var coupon = _couponApiTestFixture.DbContext.Coupons.FirstOrDefault();

            // Assert
            Assert.NotNull(coupon);

            #endregion

            #region [ Delete Coupon ]

            // Arrange
            var id = coupon.Id;
            var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons/{id}";

            // Act
            var response = await _client.DeleteAsync(requestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get Coupon ]

            // Act
            var getCoupon = _couponApiTestFixture.DbContext.Coupons.SingleOrDefault(x => x.Id == id);

            // Assert
            Assert.Null(getCoupon);

            #endregion
        }

        #endregion
    }
}