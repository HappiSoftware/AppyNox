﻿namespace AppyNox.Services.Base.Domain;

public abstract class EntityBase
{
    #region [ Fields ]

    private readonly List<IDomainEvent> _domainEvents = [];

    #endregion

    #region [ Properties ]

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    #endregion

    #region [ Protected Methods ]

    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    #endregion
}

public interface IDomainEvent
{
}