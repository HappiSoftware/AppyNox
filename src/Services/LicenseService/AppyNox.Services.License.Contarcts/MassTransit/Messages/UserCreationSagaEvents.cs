﻿using MassTransit;

namespace AppyNox.Services.License.Contarcts.MassTransit.Messages;

public record ValidateLicenseMessage(Guid CorrelationId, string LicenseKey);
public record LicenseValidatedEvent(Guid CorrelationId, bool IsValid, Guid? CompanyId, Guid? LicenseId);
public record AssignLicenseToUserMessage(Guid CorrelationId, Guid UserId, Guid LicenseId);
public record RevertApplicationUserCreationEvent(Guid CorrelationId, Guid UserId);