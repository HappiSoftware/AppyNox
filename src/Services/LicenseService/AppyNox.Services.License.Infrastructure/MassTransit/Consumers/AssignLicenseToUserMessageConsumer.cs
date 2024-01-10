using AppyNox.Services.License.Application.MediatR.Commands;
using AppyNox.Services.License.SharedEvents.Events;
using MassTransit;
using MediatR;

namespace AppyNox.Services.License.Infrastructure.MassTransit.Consumers
{
    internal sealed class AssignLicenseToUserMessageConsumer(IMediator mediator) : IConsumer<AssignLicenseToUserMessage>
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ Public Methods ]

        public async Task Consume(ConsumeContext<AssignLicenseToUserMessage> context)
        {
            await _mediator.Send(new AssignLicenseKeyToApplicationUserCommand(context.Message.LicenseId, context.Message.UserId));
        }

        #endregion
    }
}