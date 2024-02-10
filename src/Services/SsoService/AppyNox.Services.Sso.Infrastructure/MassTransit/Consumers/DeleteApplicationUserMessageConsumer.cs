using AppyNox.Services.Sso.Application.MediatR.Commands;
using AppyNox.Services.Sso.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Sso.SharedEvents.Events;
using MassTransit;
using MediatR;

namespace AppyNox.Services.Sso.Infrastructure.MassTransit.Consumers
{
    internal sealed class DeleteApplicationUserMessageConsumer(IMediator mediator) : IConsumer<DeleteApplicationUserMessage>
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ Public Methods ]

        public async Task Consume(ConsumeContext<DeleteApplicationUserMessage> context)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(context.Message.UserId));
            }
            catch (Exception ex)
            {
                throw new NoxSsoInfrastructureException(ex, (int)NoxSsoInfrastructureExceptionCode.DeleteUserConsumerError);
            }
        }

        #endregion
    }
}