using AppyNox.Services.Base.Core.MassTransit.CommonEvents;
using AppyNox.Services.Sso.Application.AsyncLocals;
using AppyNox.Services.Sso.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace AppyNox.Services.Sso.Infrastructure.MassTransit.Consumers;

internal sealed class CheckUserAvailabilityMessageConsumer(UserManager<ApplicationUser> userManager) : IConsumer<CheckUserAvailabilityMessage>
{
    #region [ Fields ]

    private readonly UserManager<ApplicationUser> _userManager = userManager;

    #endregion

    #region [ Public Methods ]

    public async Task Consume(ConsumeContext<CheckUserAvailabilityMessage> context)
    {
        try
        {
            SsoContext.IsAdmin = true;
            SsoContext.CompanyId = context.Message.CompanyId;

            var identityUser = await _userManager.FindByEmailAsync(context.Message.Email);

            if (identityUser == null)
            {
                await context.Publish(new UserIsNotAvailableEvent(
                    context.Message.CorrelationId,
                    context.Message.UserId
                ));
            }
            else
            {
                await context.Publish(new UserIsAvailableEvent(
                    context.Message.CorrelationId,
                    context.Message.UserId
                ));
            }
        }
        catch (Exception)
        {
            await context.Publish(new UserIsNotAvailableEvent(
                context.Message.CorrelationId,
                context.Message.UserId
            ));
        }
    }

    #endregion
}