using AppyNox.Services.Base.Domain.Attributes;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Entities;

public class Ticket : IEntityWithGuid, IAuditable
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    [Encrypt]
    public string Content { get; set; } = string.Empty;

    public DateTime ReportDate { get; set; }

    public string IgnoredData { get; set; } = string.Empty;

    #endregion

    #region [ IAuditable ]

    public string CreatedBy { get; } = string.Empty;

    public DateTime CreationDate { get; }

    public string? UpdatedBy { get; }

    public DateTime? UpdateDate { get; }

    #endregion

    #region [ Relations ]

    public virtual ICollection<TicketTag>? Tags { get; set; }

    #endregion
}