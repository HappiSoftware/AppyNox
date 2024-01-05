using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.MediatR.Queries;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Domain.Entities;
using Asp.Versioning;
using AutoWrapper.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class CouponsController(IMediator mediator) : Controller
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ Public Methods ]

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            return new ApiResponse(await _mediator.Send(new GetAllEntitiesQuery<CouponEntity>(queryParameters)));
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            return new ApiResponse(await _mediator.Send(new GetEntityByIdQuery<CouponEntity>(id, queryParameters)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic couponDto, string detailLevel = "Simple")
        {
            dynamic result = await _mediator.Send(new CreateEntityCommand<CouponEntity>(couponDto, detailLevel));
            var response = new
            {
                Id = result.Item1,
                Data = result.Item2
            };
            return CreatedAtAction(nameof(GetById), new { id = result.Item1 }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] dynamic couponDto, string detailLevel = "Simple")
        {
            if (id != ValidationHelpers.GetIdFromDynamicDto(couponDto))
            {
                return BadRequest();
            }

            await _mediator.Send(new GetEntityByIdQuery<CouponEntity>(id, QueryParameters.CreateForIdOnly()));

            await _mediator.Send(new UpdateEntityCommand<CouponEntity>(id, couponDto, detailLevel));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new GetEntityByIdQuery<CouponEntity>(id, QueryParameters.CreateForIdOnly()));

            await _mediator.Send(new DeleteEntityCommand<CouponEntity>(id));
            return NoContent();
        }

        #endregion
    }
}