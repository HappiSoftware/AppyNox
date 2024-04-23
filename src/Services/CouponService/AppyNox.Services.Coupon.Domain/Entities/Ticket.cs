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

    #endregion

    #region [ Relations ]

    public virtual ICollection<TicketTag>? Tags { get; set; }

    #endregion
}