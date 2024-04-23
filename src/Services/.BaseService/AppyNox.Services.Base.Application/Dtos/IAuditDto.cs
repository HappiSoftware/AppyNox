namespace AppyNox.Services.Base.Application.Dtos;

public interface IAuditDto
{
    #region [ Properties ]

    AuditInformation AuditInformation { get; set; }

    #endregion
}

public class AuditInformation
{
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}