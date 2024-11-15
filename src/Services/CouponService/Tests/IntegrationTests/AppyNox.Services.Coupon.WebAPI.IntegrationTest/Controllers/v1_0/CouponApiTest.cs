using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Base.IntegrationTests.Wrapper;
using AppyNox.Services.Base.IntegrationTests.Wrapper.Helpers;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.ValueObjects;
using AppyNox.Services.Coupon.Domain.Exceptions.Base;
using AppyNox.Services.Coupon.WebAPI.IntegrationTest.Fixtures;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;
using CouponId = AppyNox.Services.Coupon.Domain.Coupons.CouponId;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTest.Controllers.v1_0;

[Collection("CouponService Collection")]
public class CouponApiTest(CouponServiceFixture couponApiTestFixture)
{
    #region [ Fields ]

    private readonly JsonSerializerOptions _jsonSerializerOptions = couponApiTestFixture.JsonSerializerOptions;

    private readonly CouponServiceFixture _couponApiTestFixture = couponApiTestFixture;

    private readonly HttpClient _client = couponApiTestFixture.Client;

    private readonly ServiceURIs _serviceURIs = couponApiTestFixture.ServiceURIs;

    #endregion

    #region [ Public Methods ]

    [Fact]
    [Order(1)]
    public async Task GetAll_ShouldReturnSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync($"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons");
        var wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<PaginatedList<CouponDto>>(response, _jsonSerializerOptions);

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
        #region [ Get Coupon ]

        // Arrange
        var coupon = _couponApiTestFixture.DbContext.Coupons.FirstOrDefault();

        // Assert
        Assert.NotNull(coupon);

        #endregion

        #region [ Get Coupon By Id ]

        // Arrange
        var id = coupon.Id.Value;
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons/{id}";

        // Act
        var response = await _client.GetAsync(requestUri);
        var wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<CouponDto>(response, _jsonSerializerOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(wrappedResponse.Result.Data);

        #endregion
    }

    [Fact]
    [Order(3)]
    public async Task Create_ShouldAddNewCoupon()
    {
        #region [ Create Coupon ]

        // Arrange
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons";

        CouponCreateDto couponSimpleCreateDto = new()
        {
            Code = "ffff2",
            Amount = new AmountDto()
            {
                MinAmount = 12,
                DiscountAmount = 2,
            },
            Description = "string",
            CouponDetailId = new CouponDetailIdDto()
            {
                Value = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2")
            },
            Detail = "detail"
        };
        var jsonRequest = JsonSerializer.Serialize(couponSimpleCreateDto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PostAsync(requestUri, content);
        NoxApiResponse<Guid> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<Guid>(response, _jsonSerializerOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(wrappedResponse.Result.Data, Guid.Empty);

        #endregion

        #region [ Get Coupons ]

        // Act
        var coupon = _couponApiTestFixture.DbContext.Coupons.Where("Id == @0", new CouponId(wrappedResponse.Result.Data)).FirstOrDefault();

        // Assert
        Assert.NotNull(coupon);

        #endregion
    }

    [Fact]
    [Order(4)]
    public async Task Create_ShouldThrowValidationException()
    {
        #region [ Create Coupon ]

        // Arrange
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons";

        CouponCreateDto couponSimpleCreateDto = new()
        {
            Code = "ffff2",
            Amount = new AmountDto()
            {
                MinAmount = 0,
                DiscountAmount = 2,
            },
            Description = "string",
            CouponDetailId = new CouponDetailIdDto()
            {
                Value = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2")
            }
        };
        var jsonRequest = JsonSerializer.Serialize(couponSimpleCreateDto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PostAsync(requestUri, content);
        NoxApiResponse<Guid?> unwrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<Guid?>(response, _jsonSerializerOptions);

        // Assert
        Assert.True(unwrappedResponse.HasError);
        Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
        if (unwrappedResponse.Result.Error is NoxApiExceptionWrapObjectPOCO errorBody)
        {
            Assert.Equal((int)NoxApplicationExceptionCode.FluentValidationError, errorBody.ExceptionCode);
        }
        else
        {
            Assert.Fail("Expected unwrappedResponse.Result.Error to be of type NoxApiExceptionWrapObjectPOCO.");
        }

        #endregion
    }

    [Fact]
    [Order(5)]
    public async Task Create_ShouldThrowDomainException()
    {
        #region [ Create Coupon ]

        // Arrange
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons";

        CouponCreateDto couponSimpleCreateDto = new()
        {
            Code = "ffff2",
            Amount = new AmountDto()
            {
                MinAmount = 1,
                DiscountAmount = 2,
            },
            Description = "string",
            CouponDetailId = new CouponDetailIdDto()
            {
                Value = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2")
            }
        };
        var jsonRequest = JsonSerializer.Serialize(couponSimpleCreateDto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PostAsync(requestUri, content);
        NoxApiResponse<NoxApiExceptionWrapObjectPOCO> unwrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<NoxApiExceptionWrapObjectPOCO>(response, _jsonSerializerOptions);

        // Assert
        Assert.True(unwrappedResponse.HasError);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        if (unwrappedResponse.Result.Error is NoxApiExceptionWrapObjectPOCO errorBody)
        {
            Assert.Equal((int)NoxCouponDomainExceptionCode.AmountValidation, errorBody.ExceptionCode);
        }
        else
        {
            Assert.Fail("Expected unwrappedResponse.Result.Error to be of type NoxApiExceptionWrapObjectPOCO.");
        }

        #endregion
    }

    [Fact]
    [Order(6)]
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
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/coupons/{id.Value}";

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