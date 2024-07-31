using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Domain.DDD;

public abstract class AggregateMember : IAuditable
{
    #region [ Fields ]

    private readonly List<IDomainEvent> _domainEvents = [];

    #endregion

    #region [ IAuditable ]

    public string CreatedBy { get; } = string.Empty;

    public DateTime CreationDate { get; }

    public string? UpdatedBy { get; }

    public DateTime? UpdateDate { get; }

    #endregion

    #region [ Properties ]

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => [.. _domainEvents];

    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion

    #region [ Protected Methods ]

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    #endregion
}

public interface IDomainEvent : INotification
{
}