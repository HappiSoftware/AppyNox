using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.SharedEvents.Events
{
    public record StartUserCreationMessage(Guid CorrelationId, string LicenseKey, string UserName, string Email, string Password, string ConfirmPassword);
    public record CreateApplicationUserMessage(Guid CorrelationId, string UserName, string Email, string Password, string ConfirmPassword);
    public record ApplicationUserCreatedEvent(Guid CorrelationId, Guid UserId);
}
