using AutoWrapper.Wrappers;
using System.Net;
using System.Text.Json;
using AutoWrapper.Server;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using System.Text;
using AppyNox.Services.Coupon.WebAPI.IntegrationTests.Fixtures;
using AppyNox.Services.Base.IntegrationTests.URIs;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTests
{
    public class CouponApiTests(CouponApiTestFixture couponApiTestFixture) : IClassFixture<CouponApiTestFixture>
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
            var response = await _client.GetAsync($"{_serviceURIs.CouponServiceURI}/coupons");

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk();

            // Deserialize the result (assuming apiResponse.Result is a JSON array of CouponSimpleDto)
            var coupons = apiResponse?.Result is not null
                ? JsonSerializer.Deserialize<IList<CouponSimpleDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions)
                : null;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupons);
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
            var requestUri = $"{_serviceURIs.CouponServiceURI}/coupons/{id}";

            // Act
            var response = await _client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var couponObj = JsonSerializer.Deserialize<CouponSimpleDto>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

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
            var requestUri = $"{_serviceURIs.CouponServiceURI}/coupons";
            var uniqueCode = "ffff2";
            var requestBody = new
            {
                code = uniqueCode,
                discountAmount = 2,
                minAmount = 12,
                description = "string",
                couponDetailEntityId = "c2feaca4-d82a-4d2e-ba5a-667b685212b4"
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(requestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            #endregion

            #region [ Get Coupons ]

            // Act
            var coupon = _couponApiTestFixture.DbContext.Coupons.SingleOrDefault(x => x.Code == uniqueCode);

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
            var requestUri = $"{_serviceURIs.CouponServiceURI}/coupons/{id}";
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
            var requestUri = $"{_serviceURIs.CouponServiceURI}/coupons/{id}";

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