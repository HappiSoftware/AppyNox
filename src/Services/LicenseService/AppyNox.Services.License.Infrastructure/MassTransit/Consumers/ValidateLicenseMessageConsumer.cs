using AppyNox.Services.License.Application.MediatR.Commands;
using AppyNox.Services.License.Contarcts.MassTransit.Messages;
using MassTransit;
using MediatR;

namespace AppyNox.Services.License.Infrastructure.MassTransit.Consumers
{
    internal sealed class ValidateLicenseMessageConsumer(IMediator mediator) : IConsumer<ValidateLicenseMessage>
    {
        #region [ Fields ]

        private readonly IMediator _mediator = mediator;

        #endregion

        #region [ Public Methods ]

        public async Task Consume(ConsumeContext<ValidateLicenseMessage> context)
        {
            var (isValid, companyId, licenseId) = await _mediator.Send(new ValidateLicenseKeyCommand(context.Message.LicenseKey));

            await context.Publish(new LicenseValidatedEvent
            (
                context.Message.CorrelationId,
                isValid,
                companyId,
                licenseId
            ));
        }

        #endregion
    }
}