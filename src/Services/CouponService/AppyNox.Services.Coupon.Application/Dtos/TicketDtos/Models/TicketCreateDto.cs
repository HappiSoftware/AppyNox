using System.Text.Json.Serialization;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models;

public class TicketCreateDto
{
    #region [ Properties ]

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    [JsonIgnore]
    public string IgnoredData { get; set; } = string.Empty;

    public DateTime ReportDate { get; set; }

    #endregion
}