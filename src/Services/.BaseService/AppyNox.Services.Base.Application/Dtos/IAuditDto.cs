using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Application.Dtos
{
    public interface IAuditDto
    {
        #region [ Properties ]

        AuditInfo AuditInfo { get; set; }

        #endregion
    }

    public class AuditInfo : IAuditableData
    {
        #region [ IAuditableData ]

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdateDate { get; set; }

        #endregion
    }
}