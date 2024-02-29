using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Models.Basic;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Extended;

[TicketDetailLevel(TicketDataAccessDetailLevel.Extended)]
public class TicketExtendedDto : TicketSimpleDto, IAuditDto
{
    #region [ Relations ]

    public Guid Id { get; set; }

    public IEnumerable<TicketTagSimpleDto>? Tags { get; set; }

    #endregion
}