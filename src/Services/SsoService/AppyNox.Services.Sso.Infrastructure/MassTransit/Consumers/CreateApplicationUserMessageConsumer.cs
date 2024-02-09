using AppyNox.Services.Sso.Application.DTOs.ApplicationUserDTOs.Models;
using AppyNox.Services.Sso.Application.MediatR.Commands;
using AppyNox.Services.Sso.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Sso.SharedEvents.Events;
using MassTransit;
using MediatR;

namespace AppyNox.Services.Sso.Infrastructure.MassTransit.Consumers
{
    internal sealed class CreateApplicationUserMessageConsumer(IMediator mediator) : IConsumer<CreateApplicationUserMessage>
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ Public Methods ]

        public async Task Consume(ConsumeContext<CreateApplicationUserMessage> context)
        {
            try
            {
                ApplicationUserCreateDto dto = new()
                {
                    UserName = context.Message.UserName,
                    Password = context.Message.Password,
                    ConfirmPassword = context.Message.ConfirmPassword,
                    Email = context.Message.Email,
                    CompanyId = context.Message.CompanyId
                };
                var response = await _mediator.Send(new CreateUserCommand(dto));

                await context.Publish(new ApplicationUserCreatedEvent(

                    context.Message.CorrelationId,
                    response.id
                ));
            }
            catch (Exception ex)
            {
                throw new NoxSsoInfrastructureException(ex, (int)NoxSsoInfrastructureExceptionCode.CreateUserConsumerError);
            }
        }

        #endregion
    }
}