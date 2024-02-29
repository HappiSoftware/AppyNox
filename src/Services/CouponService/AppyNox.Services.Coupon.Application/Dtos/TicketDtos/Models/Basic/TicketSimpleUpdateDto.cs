using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;

[TicketDetailLevel(TicketUpdateDetailLevel.Simple)]
public class TicketSimpleUpdateDto : TicketSimpleCreateDto, IUpdateDto
{
    #region [ Properties ]

    public Guid Id { get; set; }

    #endregion
}