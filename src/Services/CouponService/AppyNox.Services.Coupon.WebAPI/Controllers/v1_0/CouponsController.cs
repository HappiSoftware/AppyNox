using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Controllers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Coupon.Domain.Coupons;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AppyNox.Services.Coupon.WebAPI.Permission.Permissions;
using CouponAggreagate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1_0;

[ApiController]
[ApiVersion(NoxVersions.v1_0)]
[Route("api/v{version:apiVersion}/coupons")]
public class CouponsController(IMediator mediator) : NoxController
{
    #region [ Fields ]

    private readonly IMediator _mediator = mediator;

    #endregion

    #region [ Public Methods ]

    [HttpGet]
    [Authorize(Coupons.View)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParametersViewModel queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetAllNoxEntitiesQuery<Domain.Coupons.Coupon>(queryParameters)));
    }

    [HttpGet("{id}")]
    [Authorize(Coupons.View)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetNoxEntityByIdQuery<Domain.Coupons.Coupon, CouponId>(new CouponId(id), queryParameters)));
    }

    [HttpPost]
    [Authorize(Coupons.Create)]
    public async Task<IActionResult> Create([FromBody] dynamic couponDto, string detailLevel = "Simple")
    {
        dynamic result = await _mediator.Send(new CreateNoxEntityCommand<Domain.Coupons.Coupon>(couponDto, detailLevel));
        var response = new
        {
            Id = result.Item1,
            CreatedObject = result.Item2
        };
        return CreatedAtAction(nameof(GetById), new { id = result.Item1 }, response);
    }

    [HttpDelete("{id}")]
    [Authorize(Coupons.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteNoxEntityCommand<CouponAggreagate, CouponId>(new CouponId(id)));
        return NoContent();
    }

    #endregion
}