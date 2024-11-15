using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;

public class TicketUpdateDto : IUpdateDto
{
    #region [ Properties ]

    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;


    #endregion
}