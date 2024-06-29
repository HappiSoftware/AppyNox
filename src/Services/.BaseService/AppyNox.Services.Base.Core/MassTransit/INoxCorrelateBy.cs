namespace AppyNox.Services.Base.Core.MassTransit;

internal interface INoxCorrelateBy
{
    Guid CorrelationId { get; }
    Guid UserId { get; }
    Guid CompanyId { get; }
}