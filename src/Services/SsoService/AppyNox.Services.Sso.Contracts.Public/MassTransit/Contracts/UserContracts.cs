using MassTransit;

namespace AppyNox.Services.Sso.Contracts.Public.MassTransit.Contracts;

public record CheckUserAvailabilityMessage(Guid CorrelationId, Guid UserId, Guid CompanyId, string Email) : CorrelatedBy<Guid>;
public record UserIsAvailableEvent(Guid CorrelationId, Guid UserId) : CorrelatedBy<Guid>;
public record UserIsNotAvailableEvent(Guid CorrelationId, Guid UserId) : CorrelatedBy<Guid>;