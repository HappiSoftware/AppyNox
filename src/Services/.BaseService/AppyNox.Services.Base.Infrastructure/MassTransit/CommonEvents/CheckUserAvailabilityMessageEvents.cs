using MassTransit;

namespace AppyNox.Services.Base.Infrastructure.MassTransit.CommonEvents;


public record CheckUserAvailabilityMessage(Guid CorrelationId, Guid UserId, string Email) : CorrelatedBy<Guid>;
public record UserIsAvailableEvent(Guid CorrelationId, Guid UserId) : CorrelatedBy<Guid>;
public record UserIsNotAvailableEvent(Guid CorrelationId, Guid UserId) : CorrelatedBy<Guid>;