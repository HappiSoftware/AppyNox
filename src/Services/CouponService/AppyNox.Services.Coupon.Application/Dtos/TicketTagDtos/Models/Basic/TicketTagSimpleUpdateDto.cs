using AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Models.Basic;

[TicketTagDetailLevel(TicketTagUpdateDetailLevel.Simple)]
public class TicketTagSimpleUpdateDto : TicketTagSimpleCreateDto
{
    #region [ Properties ]

    public Guid Id { get; set; }

    #endregion
}