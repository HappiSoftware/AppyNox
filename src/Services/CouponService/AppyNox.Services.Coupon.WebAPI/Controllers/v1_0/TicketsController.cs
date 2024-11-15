using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Controllers;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;
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
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetAllEntitiesQuery<Ticket, TicketDto>(queryParameters)));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] QueryParameters queryParameters)
    {
        return base.Ok(await _mediator.Send(new GetEntityByIdQuery<Ticket, TicketDto>(id, queryParameters)));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] TicketCreateDto ticketDto)
    {
        Guid result = await _mediator.Send(new CreateEntityCommand<Ticket, TicketCreateDto>(ticketDto));

        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Update(Guid id, [FromBody] TicketUpdateDto ticketEntity)
    {
        await _mediator.Send(new UpdateEntityCommand<Ticket, TicketUpdateDto>(id, ticketEntity));
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