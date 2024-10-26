﻿using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.License.Application.MediatR.Commands;
using AppyNox.Services.License.Contarcts.MassTransit.Messages;
using AppyNox.Services.License.Infrastructure.ExceptionExtensions;
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
            try
            {
                await _mediator.Send(new AssignLicenseKeyToApplicationUserCommand(context.Message.LicenseId, context.Message.UserId));
            }
            catch (Exception ex) when (ex is INoxException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await context.Publish(new RevertApplicationUserCreationEvent
                (
                    context.Message.CorrelationId,
                    context.Message.UserId
                ));
                throw new NoxLicenseInfrastructureException(innerException: ex);
            }
        }

        #endregion
    }
}