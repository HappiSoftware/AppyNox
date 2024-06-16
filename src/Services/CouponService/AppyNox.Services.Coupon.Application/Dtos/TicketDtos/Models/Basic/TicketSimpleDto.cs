using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;

[TicketDetailLevel(TicketDataAccessDetailLevel.Simple)]
public class TicketSimpleDto : TicketSimpleCreateDto, IAuditDto
{
    #region [ Properties ]

    public AuditInformation AuditInformation { get; set; } = default!;

    #endregion
}