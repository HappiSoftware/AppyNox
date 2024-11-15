using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Models;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;

public class TicketDto : TicketCreateDto, IAuditDto
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public AuditInformation AuditInformation { get; set; } = default!;

    #endregion

    #region [ Relations ]

    public IEnumerable<TicketTagDto>? Tags { get; set; }

    #endregion
}