using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.WebAPI.IntegrationTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;
using CouponAggregate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTest.Controllers.v1_1;

[Collection("CouponService Collection")]
public class CouponApiTestV1_1(CouponServiceFixture couponApiTestFixture)
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
    public async Task Update_ShouldUpdateCoupon()
    {
        #region [ Create and Get Coupon ]

        CouponDetail newDetail = CouponDetail.Create("test01", "testdetail");
        CouponAggregate newCoupon = CouponAggregate.Create("code", "description", "detail", new Amount(10, 15), newDetail);
        _couponApiTestFixture.DbContext.Coupons.Add(newCoupon);
        await _couponApiTestFixture.DbContext.SaveChangesAsync();

        // Act
        CouponAggregate? coupon = _couponApiTestFixture.DbContext.Coupons.Where("Id == @0", newCoupon.Id).FirstOrDefault();

        // Assert
        Assert.NotNull(coupon);
        Guid id = coupon.Id.Value;

        #endregion

        #region [ Update Coupon ]

        // Arrange
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_1}/coupons/{id}";
        int minimumAmount = coupon.Amount.MinAmount + 10;
        string detail = "This detail is modified";

        CouponExtendedUpdateDto couponExtendedUpdateDto = new()
        {
            Code = "test1",
            Amount = new AmountDto()
            {
                MinAmount = minimumAmount,
                DiscountAmount = 2,
            },
            Description = "string",
            Detail = detail,
            CouponDetailId = new CouponDetailIdDto() { Value = newDetail.Id.Value },
            Id = new CouponIdDto() { Value = id }
        };
        var jsonRequest = JsonSerializer.Serialize(couponExtendedUpdateDto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PutAsync(requestUri, content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        #endregion

        #region [ Get Coupon ]

        // Act
        if (coupon != null)
        {
            _couponApiTestFixture.DbContext.Entry(coupon).State = EntityState.Detached;
            _couponApiTestFixture.DbContext.Entry(coupon).Reload();
        }
        CouponAggregate? updatedCoupon = _couponApiTestFixture.DbContext.Coupons.Where("Id == @0", newCoupon.Id).FirstOrDefault();

        // Assert
        Assert.NotNull(updatedCoupon);
        Assert.Equal(updatedCoupon.Amount.MinAmount, minimumAmount);
        Assert.Equal(updatedCoupon.Detail, detail);
        Assert.NotNull(updatedCoupon.Audit.UpdateDate);
        Assert.False(string.IsNullOrEmpty(updatedCoupon.Audit.UpdatedBy));

        #endregion
    }

    #endregion
}