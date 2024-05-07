using AppyNox.Services.Base.API.Attributes;
using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Controllers;
using AppyNox.Services.Base.API.ViewModels;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Coupon.Domain.Entities;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppyNox.Services.Coupon.WebAPI.Controllers.v1_0;

[ApiController]
[ApiVersion(NoxVersions.v1_0)]
[Route("api/v{version:apiVersion}/tickets")]
public class TicketsController(IMediator mediator) : NoxController
{
    #region [ Fields ]

    private readonly IMediator _mediator = mediator;

    #endregion

    #region [ Public Methods ]

    [HttpGet]
    [AllowAnonymous]
    [SwaggerDynamicRequestBody(typeof(Ticket), DtoLevelMappingTypes.DataAccess, true)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParametersViewModel queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetAllEntitiesQuery<Ticket>(queryParameters)));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [SwaggerDynamicRequestBody(typeof(Ticket), DtoLevelMappingTypes.DataAccess)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParametersViewModel queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetEntityByIdQuery<Ticket>(id, queryParameters)));
    }

    [HttpPost]
    [AllowAnonymous]
    [SwaggerDynamicRequestBody(typeof(Ticket), DtoLevelMappingTypes.Create, true)]
    public async Task<IActionResult> Create([FromBody] dynamic ticketDto, string detailLevel = "Simple")
    {
        Guid result = await _mediator.Send(new CreateEntityCommand<Ticket>(ticketDto, detailLevel));

        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Update(Guid id, [FromBody] dynamic ticketEntity, string detailLevel = "Simple")
    {
        await _mediator.Send(new UpdateEntityCommand<Ticket>(id, ticketEntity, detailLevel));
        return NoContent();
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEntityCommand<Ticket>(id));
        return NoContent();
    }

    #endregion
}