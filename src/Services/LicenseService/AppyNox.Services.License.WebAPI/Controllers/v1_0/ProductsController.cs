using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.License.Domain.Entities;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.License.WebAPI.Controllers.v1_0
{
    [ApiController]
    [ApiVersion(NoxVersions.v1_0)]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController(IMediator mediator) : Controller
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ CRUD Endpoints ]

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            return Ok(await _mediator.Send(new GetAllNoxEntitiesQuery<ProductEntity>(queryParameters)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            return Ok(await _mediator.Send(new GetNoxEntityByIdQuery<ProductEntity, ProductId>(new ProductId(id), queryParameters)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic productDto, string detailLevel = "Simple")
        {
            Guid result = await _mediator.Send(new CreateNoxEntityCommand<ProductEntity>(productDto, detailLevel));

            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        // TODO Specific update methods will be added
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id, [FromBody] dynamic productDto, string detailLevel = "Simple")
        //{
        //    await _mediator.Send(new GetEntityByIdQuery<ProductEntity, ProductId>(new ProductId(id), QueryParameters.CreateForIdOnly()));

        //    await _mediator.Send(new UpdateEntityCommand<ProductEntity, ProductId>(new ProductId(id), productDto, detailLevel));
        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteNoxEntityCommand<ProductEntity, ProductId>(new ProductId(id)));
            return NoContent();
        }

        #endregion
    }
}