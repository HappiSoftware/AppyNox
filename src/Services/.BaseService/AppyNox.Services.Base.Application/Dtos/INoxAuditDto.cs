using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Application.Dtos;

public interface INoxAuditDto
{
    #region [ Properties ]

    NoxAuditDataDto Audit { get; set; }

    #endregion
}

public class NoxAuditDataDto : IAuditableData
{
    #region [ IAuditableData ]

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }

    public string UpdatedBy { get; set; } = string.Empty;

    public DateTime? UpdateDate { get; set; }

    #endregion
}