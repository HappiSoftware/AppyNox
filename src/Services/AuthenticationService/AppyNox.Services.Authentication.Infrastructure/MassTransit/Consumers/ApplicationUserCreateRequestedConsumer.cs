using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using AppyNox.Services.Authentication.Application.MediatR.Commands;
using AppyNox.Services.Authentication.SharedEvents.Events;
using MassTransit;
using MediatR;

namespace AppyNox.Services.Authentication.Infrastructure.MassTransit.Consumers
{
    public class ApplicationUserCreateRequestedConsumer(IMediator mediator) : IConsumer<ApplicationUserCreateRequested>
    {
        #region Fields

        private readonly IMediator _mediator = mediator;

        #endregion

        #region Public Methods

        public async Task Consume(ConsumeContext<ApplicationUserCreateRequested> context)
        {
            IdentityUserCreateDto dto = new()
            {
                UserName = context.Message.UserName,
                Password = context.Message.Password,
                ConfirmPassword = context.Message.ConfirmPassword,
                Email = context.Message.Email,
            };
            var response = await _mediator.Send(new CreateUserCommand(dto));

            await context.Publish<ApplicationUserCreateCompleted>(new
            {
                context.Message.CorrelationId,
                response.id
            });
        }

        #endregion
    }
}