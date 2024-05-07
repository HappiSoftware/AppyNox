using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;

[TicketDetailLevel(TicketDataAccessDetailLevel.Simple)]
public class TicketSimpleDto : TicketSimpleCreateDto, IAuditDto
{
    #region [ Properties ]

    public AuditInformation AuditInformation { get; set; } = default!;

    [JsonIgnore] // Get method testing for swagger
    public new string IgnoredData { get; set; } = string.Empty;

    #endregion
}