using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Controllers;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.MediatR;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models;
using AppyNox.Services.Coupon.Domain.Coupons;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AppyNox.Services.Coupon.Application.Permission.Permissions;
using CouponAggreagate = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1_0;

[ApiController]
[ApiVersion(NoxVersions.v1_0)]
[Route("api/v{version:apiVersion}/coupons")]
public class CouponsController(IMediator mediator, INoxApiLogger<CouponsController> logger) : NoxController
{
    #region [ Fields ]

    private readonly IMediator _mediator = mediator;

    #endregion

    #region [ Public Methods ]

    [HttpGet]
    [Authorize(Coupons.View)]
    public async Task<ActionResult<NoxApiResponse<IEnumerable<CouponDto>>>> GetAll([FromQuery] QueryParameters queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetAllNoxEntitiesQuery<CouponAggreagate, CouponDto>(queryParameters)));
    }

    [HttpGet("{id}")]
    [Authorize(Coupons.View)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParameters queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetNoxEntityByIdQuery<CouponAggreagate, CouponId, CouponDto>(new CouponId(id), queryParameters)));
    }

    [HttpPost]
    [Authorize(Coupons.Create)]
    public async Task<IActionResult> Create([FromBody] CouponCreateDto couponDto)
    {
        Guid result = await _mediator.Send(new CreateNoxEntityCommand<CouponAggreagate, CouponCreateDto>(couponDto));

        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Coupons.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var t1 = new NoxCommandAction(NoxCommandActionMethodTest1);
        var t2 = new NoxCommandAction(NoxCommandActionMethodTest2);
        NoxCommandExtensions extensions = new(t1, t2);
        await _mediator.Send(new DeleteNoxEntityCommand<CouponAggreagate, CouponId>(new CouponId(id), false, extensions));
        return NoContent();
    }

    // Below methods are for manual testing
    private void NoxCommandActionMethodTest1()
    {
        Console.WriteLine(1);
    }

    private void NoxCommandActionMethodTest2()
    {
        Console.WriteLine(2);
    }

    #endregion
}