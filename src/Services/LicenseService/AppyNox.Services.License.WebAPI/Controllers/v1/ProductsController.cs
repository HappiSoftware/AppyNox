using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.License.Domain.Entities;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.License.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion(NoxVersions.v1_0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController(IMediator mediator) : Controller
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ CRUD Endpoints ]

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            return Ok(await _mediator.Send(new GetAllEntitiesQuery<ProductEntity>(queryParameters)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            return Ok(await _mediator.Send(new GetEntityByIdQuery<ProductEntity>(id, queryParameters)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic productDto, string detailLevel = "Simple")
        {
            dynamic result = await _mediator.Send(new CreateEntityCommand<ProductEntity>(productDto, detailLevel));
            var response = new
            {
                Id = result.Item1,
                CreatedObject = result.Item2
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