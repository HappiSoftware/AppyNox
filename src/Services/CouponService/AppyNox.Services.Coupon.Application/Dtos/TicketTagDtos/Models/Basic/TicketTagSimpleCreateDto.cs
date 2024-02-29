using AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.Models.Basic;

[TicketTagDetailLevel(TicketTagCreateDetailLevel.Simple)]
public class TicketTagSimpleCreateDto
{
    #region [ Properties ]

    public string Description { get; set; } = string.Empty;

    #endregion
}