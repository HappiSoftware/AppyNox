using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Controllers;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using AppyNox.Services.Coupon.Application.MediatR.Commands;
using AppyNox.Services.Coupon.Domain.Coupons;
using AppyNox.Services.Coupon.Infrastructure.Authentication;
using AppyNox.Services.License.Client;
using AppyNox.Services.License.Contarcts.MassTransit.Messages;
using Asp.Versioning;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AppyNox.Services.Coupon.Application.Permission.Permissions;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1_1;

[ApiController]
[ApiVersion(NoxVersions.v1_1)]
[Route("api/v{version:apiVersion}/coupons")]
public class CouponsController(
    IMediator mediator,
    IPublishEndpoint publishEndpoint,
    ILicenseServiceClient licenseServiceClient,
    ICouponTokenManager couponTokenManager) : NoxController
{
    #region [ Fields ]

    private readonly IMediator _mediator = mediator;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILicenseServiceClient _licenseServiceClient = licenseServiceClient;
    private readonly ICouponTokenManager _couponTokenManager = couponTokenManager;

    #endregion

    #region [ Routes ]

    [HttpGet]
    [Authorize(Coupons.View)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters)
    {
        PaginatedList<CouponDto> response = await _mediator.Send(new GetAllNoxEntitiesQuery<Domain.Coupons.Coupon, CouponDto>(queryParameters));

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(Coupons.View)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParameters queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetNoxEntityByIdQuery<Domain.Coupons.Coupon, CouponId, CouponDto>(new CouponId(id), queryParameters)));
    }

    [HttpPost]
    [Authorize(Coupons.Create)]
    public async Task<IActionResult> Create([FromBody] CouponCompositeCreateDto couponDto)
    {
        Guid result = await _mediator.Send(new CreateNoxEntityCommand<Domain.Coupons.Coupon, CouponCompositeCreateDto>(couponDto));

        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Coupons.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CouponUpdateDto couponDto)
    {
        await _mediator.Send(new UpdateCouponCommand(id, couponDto));
        return NoContent();
    }

    [HttpGet]
    [Authorize("Coupons.View.Admin")]
    [Route("/api/v{version:apiVersion}/coupons/admin-test")]
    public IActionResult TestAdminEndpoint()
    {
        return Ok("Request successful");
    }

    [HttpGet]
    [Authorize("Coupons.View.Admin")]
    [Route("/api/v{version:apiVersion}/coupons/mt-test")]
    public IActionResult TestMassTransit()
    {
        _publishEndpoint.Publish(new ValidateLicenseMessage(NoxContext.CorrelationId, "test"));
        return Ok("Request successful");
    }

    [HttpGet]
    [Authorize("Coupons.View.Admin")]
    [Route("/api/v{version:apiVersion}/coupons/mt-test2")]
    public IActionResult TestMassTransitDataRequest()
    {
        _licenseServiceClient.GetLicenseById("");
        return Ok("Request successful");
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("/api/v{version:apiVersion}/coupons/authenticate")]
    public IActionResult GetCouponDummyToken()
    {
        return Ok(_couponTokenManager.CreateToken());
    }

    [HttpGet]
    [Authorize(CouponCustomJwt.CustomView)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<CouponDto>))]
    [Route("/api/v{version:apiVersion}/coupons/getwithcustomjwt")]
    public async Task<IActionResult> GetAllCustomAsync([FromQuery] QueryParameters queryParameters)
    {
        PaginatedList<CouponDto> response = await _mediator.Send(new GetAllNoxEntitiesQuery<Domain.Coupons.Coupon, CouponDto>(queryParameters));

        return Ok(response);
    }

    #endregion
}