using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Domain;

public abstract class AggregateRoot : IAuditable
{
    #region [ Fields ]

    private readonly List<IDomainEvent> _domainEvents = [];

    #endregion

    #region [ Properties ]

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion

    #region [ Protected Methods ]

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    #endregion
}

public interface IDomainEvent : INotification
{
}