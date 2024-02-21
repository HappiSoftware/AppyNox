using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.Domain.Interfaces;

public interface INoxAuditableData
{
    #region [ Properties ]

    NoxAuditData Audit { get; }

    #endregion

    #region [ Public Methods ]

    void AddAuditInformation(string createdBy, DateTime creationDate);

    void UpdateAuditInformation(string updatedBy, DateTime? updateDate);

    #endregion
}

public sealed record NoxAuditData
{
    public string CreatedBy { get; private set; }

    public DateTime CreationDate { get; private set; }

    public string UpdatedBy { get; private set; }

    public DateTime? UpdateDate { get; private set; }
    public NoxAuditData(string createdBy, DateTime creationDate, string updatedBy, DateTime? updateDate)
    {
        if (updateDate.HasValue && string.IsNullOrEmpty(updatedBy))
        {
            throw new NoxDomainException("'AuditableData' - 'UpdatedBy must be provided when UpdateDate is set.'", (int)NoxDomainExceptionCode.AuditableDataValidation);
        }
        CreatedBy = createdBy;
        CreationDate = creationDate;
        UpdatedBy = updatedBy;
        UpdateDate = updateDate;
    }

    public static NoxAuditData AddCreation(string createdBy, DateTime creationDate)
    {
        return new NoxAuditData(createdBy, creationDate, string.Empty, null);
    }

    public NoxAuditData UpdateModification(string updatedBy, DateTime? updateDate)
    {
        return this with { UpdatedBy = updatedBy, UpdateDate = updateDate };
    }
}