using AppyNox.Services.Base.API.Filters;
using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.License.Domain.Entities;
using Asp.Versioning;
using AutoWrapper.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.License.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [JwtTokenValidate]
    public class ProductsController(IMediator mediator) : Controller
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ CRUD Endpoints ]

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            return new ApiResponse(await _mediator.Send(new GetAllEntitiesQuery<ProductEntity>(queryParameters)));
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            return new ApiResponse(await _mediator.Send(new GetEntityByIdQuery<ProductEntity>(id, queryParameters)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic productDto, string detailLevel = "Simple")
        {
            dynamic result = await _mediator.Send(new CreateEntityCommand<ProductEntity>(productDto, detailLevel));
            var response = new
            {
                Id = result.Item1,
                Data = result.Item2
            };
            return CreatedAtAction(nameof(GetById), new { id = result.Item1 }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] dynamic productDto, string detailLevel = "Simple")
        {
            if (id != ValidationHelpers.GetIdFromDynamicDto(productDto))
            {
                return BadRequest();
            }

            await _mediator.Send(new GetEntityByIdQuery<ProductEntity>(id, QueryParameters.CreateForIdOnly()));

            await _mediator.Send(new UpdateEntityCommand<ProductEntity>(id, productDto, detailLevel));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new GetEntityByIdQuery<ProductEntity>(id, QueryParameters.CreateForIdOnly()));

            await _mediator.Send(new DeleteEntityCommand<ProductEntity>(id));
            return NoContent();
        }

        #endregion
    }
}