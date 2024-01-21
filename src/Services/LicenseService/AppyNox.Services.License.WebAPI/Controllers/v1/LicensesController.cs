using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.License.Application.MediatR.Commands;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.WebAPI.Permission;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.License.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion(NoxVersions.v1_0)]
    [Route("api/[controller]")]
    public class LicensesController(IMediator mediator) : Controller
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ CRUD Endpoints ]

        [HttpGet]
        [Authorize(Permissions.Licenses.View)]
        public async Task<IActionResult> GetAll([FromQuery] QueryParametersViewModel queryParameters)
        {
            return Ok(await _mediator.Send(new GetAllEntitiesQuery<LicenseEntity>(queryParameters)));
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Licenses.View)]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
        {
            return Ok(await _mediator.Send(new GetEntityByIdQuery<LicenseEntity>(id, queryParameters)));
        }

        [HttpPost]
        [Authorize(Permissions.Licenses.Create)]
        public async Task<IActionResult> Create([FromBody] dynamic licenseDto, string detailLevel = "Simple")
        {
            dynamic result = await _mediator.Send(new CreateEntityCommand<LicenseEntity>(licenseDto, detailLevel));
            var response = new
            {
                Id = result.Item1,
                Data = result.Item2
            };
            return CreatedAtAction(nameof(GetById), new { id = result.Item1 }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.Licenses.Edit)]
        public async Task<IActionResult> Update(Guid id, [FromBody] dynamic licenseDto, string detailLevel = "Simple")
        {
            if (id != ValidationHelpers.GetIdFromDynamicDto(licenseDto))
            {
                return BadRequest();
            }

            await _mediator.Send(new GetEntityByIdQuery<LicenseEntity>(id, QueryParameters.CreateForIdOnly()));

            await _mediator.Send(new UpdateEntityCommand<LicenseEntity>(id, licenseDto, detailLevel));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.Licenses.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new GetEntityByIdQuery<LicenseEntity>(id, QueryParameters.CreateForIdOnly()));

            await _mediator.Send(new DeleteEntityCommand<LicenseEntity>(id));
            return NoContent();
        }

        #endregion

        #region [ Custom Endpoints ]

        [HttpGet("validate/{licenseKey}")]
        public async Task<IActionResult> ValidateLicenseKey(string licenseKey)
        {
            var response = await _mediator.Send(new ValidateLicenseKeyCommand(licenseKey));

            if (response.isValid)
            {
                return Ok("License key is valid and has room for additional users.");
            }
            else { return BadRequest(); }
        }

        #endregion
    }
}