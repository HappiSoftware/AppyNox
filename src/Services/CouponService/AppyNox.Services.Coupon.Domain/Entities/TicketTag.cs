using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Entities;

public class TicketTag : IEntityWithGuid
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string Description { get; set; } = string.Empty;

    #endregion

    #region [ Relations ]

    public Guid TicketId { get; set; }

    public Ticket Ticket { get; set; } = default!;

    #endregion
}