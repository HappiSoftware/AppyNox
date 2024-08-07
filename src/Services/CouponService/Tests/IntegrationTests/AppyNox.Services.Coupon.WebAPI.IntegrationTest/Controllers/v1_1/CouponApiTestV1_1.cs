using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.IntegrationTests.Stubs;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Domain.Coupons.Builders;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
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
        NoxInfrastructureLoggerStub<UnitOfWorkBase> logger = new();
        UnitOfWork unitOfWork = new(_couponApiTestFixture.DbContext, logger);
        CouponDetail newDetail = new CouponDetailBuilder().WithDetails("test01", "testdetail").Build();
        CouponAggregate newCoupon = new CouponBuilder().WithDetails("code", "description", "detail")
                                                       .WithAmount(10, 15)
                                                       .WithCouponDetail(newDetail)
                                                       .MarkAsBulkCreate()
                                                       .Build();
        _couponApiTestFixture.DbContext.Coupons.Add(newCoupon);
        await unitOfWork.SaveChangesAsync();

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
        var updateDate = _couponApiTestFixture.DbContext.Entry(updatedCoupon).Property("UpdateDate").CurrentValue as DateTime?;
        var updatedBy = _couponApiTestFixture.DbContext.Entry(updatedCoupon).Property("UpdatedBy").CurrentValue as string;
        Assert.Equal(updatedCoupon.Amount.MinAmount, minimumAmount);
        Assert.Equal(updatedCoupon.Detail, detail);
        Assert.NotNull(updateDate);
        Assert.False(string.IsNullOrEmpty(updatedBy));

        #endregion
    }

    #endregion
}