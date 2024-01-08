using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.License.SharedEvents.Events
{
    public record ValidateLicenseMessage(Guid CorrelationId, string LicenseKey);
    public record LicenseValidatedEvent(Guid CorrelationId, bool IsValid);
    public record AssignLicenseToUserMessage(Guid CorrelationId, Guid UserId, string? LicenseKey);
}
