using AppyNox.Services.Coupon.IntegrationTests.WebApiFactories;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Text.Json;
using AutoWrapper.Server;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using NLog.Targets;
using System.Text;
using static AppyNox.Services.Coupon.WebAPI.Helpers.Permissions.Permissions;

namespace AppyNox.Services.Coupon.IntegrationTests
{
    public class CouponApiTests
    {
        #region [ Fields ]

        private readonly WebApiFactory _factory;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        #endregion

        #region [ Public Constructors ]

        public CouponApiTests()
        {
            _factory = new WebApiFactory();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        #endregion

        #region [ Public Methods ]

        [Fact]
        public async Task GetAll_ShouldReturnSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();
            var requestUri = "/api/coupons";

            // Act
            var response = await client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var coupons = JsonSerializer.Deserialize<IList<CouponSimpleDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

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
            var client = _factory.CreateDefaultClient();
            var requestUri = "/api/coupons?Access=Update&PageSize=1&PageNumber=1";

            // Act
            var response = await client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var coupons = JsonSerializer.Deserialize<IList<CouponSimpleUpdateDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupons);
            Assert.Equal(1, coupons.Count);

            #endregion

            #region [ Get Coupon By Id ]

            // Arrange
            var id = coupons[0].Id;
            requestUri = $"/api/coupons/{id}";

            // Act
            response = await client.GetAsync(requestUri);
            jsonResponse = await response.Content.ReadAsStringAsync();
            apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var coupon = JsonSerializer.Deserialize<CouponSimpleDto>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupon);

            #endregion
        }

        [Fact]
        public async Task Create_ShouldAddNewCoupon()
        {
            #region [ Create Coupon ]

            // Arrange
            var client = _factory.CreateDefaultClient();
            var requestUri = "/api/coupons";
            var requestBody = new
            {
                code = "ffff2",
                discountAmount = 2,
                minAmount = 12,
                description = "string",
                couponDetailEntityId = "c2feaca4-d82a-4d2e-ba5a-667b685212b4"
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(requestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            #endregion

            #region [ Get Coupons ]

            // Act
            response = await client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var coupons = JsonSerializer.Deserialize<IList<CouponSimpleUpdateDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupons);

            #endregion
        }

        [Fact]
        public async Task Update_ShouldUpdateEntity()
        {
            #region [ Get Coupon ]

            // Arrange
            var client = _factory.CreateDefaultClient();
            var requestUri = "/api/coupons?Access=Update&PageSize=1&PageNumber=1";

            // Act
            var response = await client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var coupons = JsonSerializer.Deserialize<IList<CouponSimpleUpdateDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupons);
            Assert.Equal(1, coupons.Count);

            #endregion

            #region [ Update Coupon ]

            // Arrange
            var oldCoupon = coupons[0];
            var id = coupons[0].Id;
            requestUri = $"/api/coupons/{id}";
            var requestBody = new
            {
                code = oldCoupon.Code,
                discountAmount = oldCoupon.DiscountAmount + 1,
                minAmount = oldCoupon.MinAmount + 1,
                description = $"{oldCoupon}++",
                couponDetailEntityId = oldCoupon.CouponDetailEntityId,
                id = oldCoupon.Id
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Act
            response = await client.PutAsync(requestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get Coupon ]

            // Arrange
            requestUri = $"/api/coupons/{id}";

            // Act
            response = await client.GetAsync(requestUri);
            jsonResponse = await response.Content.ReadAsStringAsync();
            apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var coupon = JsonSerializer.Deserialize<CouponSimpleDto>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupon);
            Assert.Equal(oldCoupon.Code, coupon.Code);
            Assert.Equal(oldCoupon.DiscountAmount + 1, coupon.DiscountAmount);
            Assert.Equal(oldCoupon.MinAmount + 1, coupon.MinAmount);

            #endregion
        }

        [Fact]
        public async Task Delete_ShouldDeleteEntity()
        {
            #region [ Get Coupon ]

            // Arrange
            var client = _factory.CreateDefaultClient();
            var requestUri = "/api/coupons?Access=Update&PageSize=1&PageNumber=2";

            // Act
            var response = await client.GetAsync(requestUri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(jsonResponse);
            apiResponse.ValidateOk(); // This line covers if apiResponse.Result is null so we can use null forgiving operator on the next line
            var coupons = JsonSerializer.Deserialize<IList<CouponSimpleUpdateDto>>(apiResponse.Result.ToString()!, _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(coupons);

            #endregion

            #region [ Delete Coupon ]

            // Arrange
            var id = coupons[0].Id;
            requestUri = $"/api/coupons/{id}";

            // Act
            response = await client.DeleteAsync(requestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            #endregion

            #region [ Get Coupon ]

            // Act
            response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            #endregion
        }

        #endregion
    }
}