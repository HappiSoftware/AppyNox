using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;

[TicketDetailLevel(TicketCreateDetailLevel.Simple)]
public class TicketSimpleCreateDto
{
    #region [ Properties ]

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    #endregion
}