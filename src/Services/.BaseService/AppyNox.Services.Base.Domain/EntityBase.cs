using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Domain;

public abstract class EntityBase : INoxAuditableData
{
    #region [ Fields ]

    private readonly List<IDomainEvent> _domainEvents = [];

    #endregion

    #region [ Properties ]

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    #endregion

    #region [ IAuditableData ]

    public NoxAuditData Audit { get; private set; } = default!;

    #endregion

    #region [ Protected Methods ]

    public void UpdateAuditInformation(string updatedBy, DateTime? updateDate)
    {
        Audit = Audit.UpdateModification(updatedBy, updateDate);
    }

    public void AddAuditInformation(string createdBy, DateTime creationDate)
    {
        Audit = NoxAuditData.AddCreation(createdBy, creationDate);
    }

    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    #endregion
}

public interface IDomainEvent
{
}