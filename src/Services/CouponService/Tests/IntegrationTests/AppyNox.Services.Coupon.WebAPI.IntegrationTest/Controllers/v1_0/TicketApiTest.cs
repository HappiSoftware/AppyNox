using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Base.IntegrationTests.Stubs;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Base.IntegrationTests.Wrapper;
using AppyNox.Services.Base.IntegrationTests.Wrapper.Helpers;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Extended;
using AppyNox.Services.Coupon.Domain.Entities;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AppyNox.Services.Coupon.WebAPI.IntegrationTest.Fixtures;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTest.Controllers.v1_0;

[Collection("CouponService Collection")]
public class TicketApiTest(CouponServiceFixture couponApiTestFixture)
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
        var response = await _client.GetAsync($"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/tickets");

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var coupons = NoxResponseUnwrapper.UnwrapData<PaginatedList<TicketExtendedDto>>(jsonResponse, _jsonSerializerOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(coupons.Items);
    }

    [Fact]
    [Order(2)]
    public async Task GetById_ShouldReturnSuccessStatusCode()
    {
        #region [ Get Ticket ]

        // Arrange
        var ticket = _couponApiTestFixture.DbContext.Tickets.FirstOrDefault();

        // Assert
        Assert.NotNull(ticket);

        #endregion

        #region [ Get Ticket By Id ]

        // Arrange
        var id = ticket.Id;
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/tickets/{id}";

        // Act
        var response = await _client.GetAsync(requestUri);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var ticketObj = NoxResponseUnwrapper.UnwrapData<CouponSimpleDto>(jsonResponse, _jsonSerializerOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(ticketObj);

        #endregion
    }

    [Fact]
    [Order(3)]
    public async Task Create_ShouldAddNewTicket()
    {
        #region [ Create Ticket ]

        // Arrange
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/tickets";

        TicketSimpleCreateDto ticketSimpleCreateDto = new()
        {
            Title = "new title",
            Content = "new content",
            ReportDate = DateTime.UtcNow,
        };
        var jsonRequest = JsonSerializer.Serialize(ticketSimpleCreateDto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PostAsync(requestUri, content);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        Guid id = NoxResponseUnwrapper.UnwrapData<Guid>(jsonResponse, _jsonSerializerOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(id, Guid.Empty);

        #endregion

        #region [ Get Tickets ]

        // Act
        var ticket = _couponApiTestFixture.DbContext.Tickets.Where(x => x.Id == id).FirstOrDefault();

        // Assert
        Assert.NotNull(ticket);

        #endregion
    }

    [Fact]
    [Order(4)]
    public async Task Create_ShouldThrowValidationException()
    {
        #region [ Create Coupon ]

        // Arrange
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/tickets";

        TicketSimpleCreateDto ticketSimpleCreateDto = new()
        {
            Title = string.Empty,
            Content = "new content"
        };
        var jsonRequest = JsonSerializer.Serialize(ticketSimpleCreateDto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PostAsync(requestUri, content);
        NoxApiResponse unwrappedResponse = await NoxResponseUnwrapper.UnwrapResponse(response, _jsonSerializerOptions);

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
    public async Task Update_ShouldUpdateTicket()
    {
        #region [ Create and Get Ticket ]
        NoxInfrastructureLoggerStub<UnitOfWorkBase> logger = new();
        UnitOfWork unitOfWork = new(_couponApiTestFixture.DbContext, logger);
        Ticket ticket = new()
        {
            Title = "title",
            Content = "content"
        };
        _couponApiTestFixture.DbContext.Tickets.Add(ticket);
        await unitOfWork.SaveChangesAsync();

        // Act
        Ticket? ticketEntity = _couponApiTestFixture.DbContext.Tickets.Where(x => x.Id == ticket.Id).FirstOrDefault();

        // Assert
        Assert.NotNull(ticketEntity);
        Guid id = ticketEntity.Id;

        #endregion

        #region [ Update Ticket ]

        // Arrange
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/tickets/{id}";
        string updatedTitle = "updated title";
        string updatedContent = "updated content";

        TicketSimpleUpdateDto ticketSimpleUpdateDto = new()
        {
            Title = updatedTitle,
            Content = updatedContent,
            Id = id
        };
        var jsonRequest = JsonSerializer.Serialize(ticketSimpleUpdateDto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PutAsync(requestUri, content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        #endregion

        #region [ Get Ticket ]

        // Act
        if (ticketEntity != null)
        {
            _couponApiTestFixture.DbContext.Entry(ticketEntity).State = EntityState.Detached;
            _couponApiTestFixture.DbContext.Entry(ticketEntity).Reload();
        }
        Ticket? updatedTicket = _couponApiTestFixture.DbContext.Tickets.Where(x => x.Id == id).FirstOrDefault();

        // Assert
        Assert.NotNull(updatedTicket);
        var updateDate = _couponApiTestFixture.DbContext.Entry(updatedTicket).Property("UpdateDate").CurrentValue as DateTime?;
        var updatedBy = _couponApiTestFixture.DbContext.Entry(updatedTicket).Property("UpdatedBy").CurrentValue as string;
        Assert.Equal(updatedTicket.Title, updatedTitle);
        Assert.Equal(updatedTicket.Content, updatedContent); // Encrypted data
        Assert.NotNull(updateDate);
        Assert.False(string.IsNullOrEmpty(updatedBy));

        #endregion
    }

    [Fact]
    [Order(6)]
    public async Task Delete_ShouldDeleteEntity()
    {
        #region [ Get Ticket ]

        // Arrange
        var ticket = _couponApiTestFixture.DbContext.Tickets.FirstOrDefault();

        // Assert
        Assert.NotNull(ticket);

        #endregion

        #region [ Delete Ticket ]

        // Arrange
        var id = ticket.Id;
        var requestUri = $"{_serviceURIs.CouponServiceURI}/v{NoxVersions.v1_0}/tickets/{id}";

        // Act
        var response = await _client.DeleteAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        #endregion

        #region [ Get Ticket ]

        // Act
        var getTicket = _couponApiTestFixture.DbContext.Tickets.SingleOrDefault(x => x.Id == id);

        // Assert
        Assert.Null(getTicket);

        #endregion
    }

    #endregion
}