using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;

[TicketDetailLevel(TicketCreateDetailLevel.Simple)]
public class TicketSimpleCreateDto
{
    #region [ Properties ]

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    [JsonIgnore]
    public string IgnoredData { get; set; } = string.Empty;

    public DateTime ReportDate { get; set; }

    #endregion
}