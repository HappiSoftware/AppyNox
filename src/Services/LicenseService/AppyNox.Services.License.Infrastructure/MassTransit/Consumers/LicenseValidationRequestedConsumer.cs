using AppyNox.Services.License.Application.MediatR.Commands;
using AppyNox.Services.License.SharedEvents.Events;
using MassTransit;
using MediatR;

namespace AppyNox.Services.License.Infrastructure.MassTransit.Consumers
{
    public class LicenseValidationRequestedConsumer(IMediator mediator) : IConsumer<LicenseValidationRequested>
    {
        #region Fields

        private readonly IMediator _mediator = mediator;

        #endregion

        #region Public Methods

        public async Task Consume(ConsumeContext<LicenseValidationRequested> context)
        {
            bool isValid = await _mediator.Send(new ValidateLicenseKeyCommand(context.Message.LicenseKey));

            await context.Publish<LicenseValidationCompleted>(new
            {
                context.Message.CorrelationId,
                IsValid = isValid
            });
        }

        #endregion
    }
}