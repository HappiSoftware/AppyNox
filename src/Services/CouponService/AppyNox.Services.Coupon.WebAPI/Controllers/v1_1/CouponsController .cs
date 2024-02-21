using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Controllers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Application.MediatR.Commands;
using AppyNox.Services.Coupon.Domain.Coupons;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AppyNox.Services.Coupon.WebAPI.Permission.Permissions;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1_1;

[ApiController]
[ApiVersion(NoxVersions.v1_1)]
[Route("api/v{version:apiVersion}/coupons")]
public class CouponsController(IMediator mediator) : NoxController
{
    #region [ Fields ]

    private readonly IMediator _mediator = mediator;

    #endregion

    #region [ Routes ]

    [HttpGet]
    [Authorize(Coupons.View)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypedPaginatedList<CouponSimpleDto>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParametersViewModelBasic queryParameters)
    {
        PaginatedList response = await _mediator.Send(new GetAllNoxEntitiesQuery<Domain.Coupons.Coupon>(queryParameters));

        return Ok(response.ConvertToTypedPaginatedList<CouponSimpleDto>());
    }

    [HttpGet("{id}")]
    [Authorize(Coupons.View)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CouponSimpleDto))]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParametersViewModelBasic queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetNoxEntityByIdQuery<Domain.Coupons.Coupon, CouponId>(new CouponId(id), queryParameters)));
    }

    [HttpPut("{id}")]
    [Authorize(Coupons.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CouponExtendedUpdateDto couponDto)
    {
        await _mediator.Send(new UpdateCouponCommand(id, couponDto));
        return NoContent();
    }

    #endregion
}