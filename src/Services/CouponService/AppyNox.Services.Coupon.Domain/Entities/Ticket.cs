using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Entities;

public class Ticket : IEntityWithGuid, IAuditableData
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime ReportDate { get; set; }

    #endregion

    #region [ Relations ]

    public ICollection<TicketTag>? Tags { get; set; }

    #endregion

    #region [ IAuditableData ]

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }

    public string UpdatedBy { get; set; } = string.Empty;

    public DateTime? UpdateDate { get; set; }

    #endregion
}